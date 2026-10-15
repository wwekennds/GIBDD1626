using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp12
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnCreateDriver_Click(object sender, EventArgs e)
        {
            FormCreateDriver formCreateDriver = new FormCreateDriver();
            formCreateDriver.Show();

        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnCards_Click(object sender, EventArgs e)
        {
            FormCardsDriver formCardsDriver = new FormCardsDriver();
            formCardsDriver.Show();
        }

        private void btnCreateVehicle_Click(object sender, EventArgs e)
        {
            FormCreateVehicle formCreateVehicle = new FormCreateVehicle();
            formCreateVehicle.Show();
        }

        private void btnFine_Click(object sender, EventArgs e)
        {
            FinesForm formFine = new FinesForm();
            formFine.Show();
        }

        private void btnExportCSV_Click(object sender, EventArgs e)
        {
            string outputDirectory = null;

            try
            {
                using (var connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;
                     Initial Catalog=gibddDB;
                     Integrated Security=SSPI;"))
                {
                    connection.Open();
                    EnsureFineStatusColumn(connection);

                    using (var transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                    {
                        try
                        {
                            var fines = LoadUnpaidFines(connection, transaction);
                            if (fines.Count == 0)
                            {
                                transaction.Commit();
                                MessageBox.Show(
                                    "Нет неоплаченных штрафов для экспорта.",
                                    "Экспорт в ФССП",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                                return;
                            }

                            outputDirectory = CreateExportDirectory();
                            int filesCount = ExportFinesByDriver(fines, outputDirectory);
                            MarkFinesAsTransferred(connection, transaction, fines);
                            transaction.Commit();

                            MessageBox.Show(
                                $"Сформировано файлов: {filesCount}\n" +
                                $"Штрафов отправлено в ФССП: {fines.Count}\n\n" +
                                $"Папка экспорта: {outputDirectory}",
                                "Экспорт завершён",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string message = $"Не удалось экспортировать штрафы: {ex.Message}";
                if (!string.IsNullOrEmpty(outputDirectory) && Directory.Exists(outputDirectory))
                {
                    try
                    {
                        Directory.Delete(outputDirectory, true);
                    }
                    catch (Exception cleanupException)
                    {
                        message += $"\nНе удалось удалить неполную папку экспорта: {cleanupException.Message}";
                    }
                }

                MessageBox.Show(message, "Ошибка экспорта", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void EnsureFineStatusColumn(SqlConnection connection)
        {
            const string sql = @"
IF COL_LENGTH(N'dbo.Fines', N'Status') IS NULL
BEGIN
    ALTER TABLE dbo.Fines
    ADD Status NVARCHAR(50) NOT NULL
        CONSTRAINT DF_Fines_Status DEFAULT N'Не оплачен' WITH VALUES;
END";

            using (var command = new SqlCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        private static List<FineExportRecord> LoadUnpaidFines(
            SqlConnection connection,
            SqlTransaction transaction)
        {
            const string sql = @"
SELECT
    f.Id AS FineId,
    c.Id AS DriverId,
    c.Surname,
    c.Name,
    c.Patromic,
    c.Passport,
    c.Registration_city,
    c.Registration_street_address,
    c.Residence_city,
    c.Residence_street_address,
    v.Series,
    v.Number,
    v.RegionCode,
    f.ViolationDate,
    f.ArticleCode,
    f.FineAmount,
    f.ExternalId,
    f.AccidentType
FROM dbo.Fines AS f WITH (UPDLOCK, HOLDLOCK)
INNER JOIN dbo.Vehicles AS v ON v.Id = f.VehicleId
INNER JOIN dbo.Clients AS c ON c.Id = v.ClientId
WHERE f.Status = @UnpaidStatus
ORDER BY c.Surname, c.Name, c.Patromic, f.ViolationDate;";

            var fines = new List<FineExportRecord>();
            using (var command = new SqlCommand(sql, connection, transaction))
            {
                command.Parameters.Add("@UnpaidStatus", SqlDbType.NVarChar, 50).Value = "Не оплачен";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        fines.Add(new FineExportRecord
                        {
                            FineId = reader.GetGuid(reader.GetOrdinal("FineId")),
                            DriverId = reader.GetGuid(reader.GetOrdinal("DriverId")),
                            Surname = ReadString(reader, "Surname"),
                            Name = ReadString(reader, "Name"),
                            Patronymic = ReadString(reader, "Patromic"),
                            Passport = ReadString(reader, "Passport"),
                            RegistrationCity = ReadString(reader, "Registration_city"),
                            RegistrationAddress = ReadString(reader, "Registration_street_address"),
                            ResidenceCity = ReadString(reader, "Residence_city"),
                            ResidenceAddress = ReadString(reader, "Residence_street_address"),
                            VehicleNumber = ReadString(reader, "Series") +
                                ReadString(reader, "Number") +
                                ReadString(reader, "RegionCode"),
                            ViolationDate = reader.GetDateTime(reader.GetOrdinal("ViolationDate")),
                            Article = ReadString(reader, "ArticleCode"),
                            Amount = reader.GetDecimal(reader.GetOrdinal("FineAmount")),
                            ExternalId = ReadString(reader, "ExternalId"),
                            AccidentType = ReadString(reader, "AccidentType")
                        });
                    }
                }
            }

            return fines;
        }

        private static string ReadString(SqlDataReader reader, string columnName)
        {
            int ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? string.Empty : reader.GetString(ordinal);
        }

        private static string CreateExportDirectory()
        {
            string projectDirectory = FindProjectDirectory();
            string baseName = "Экспорт_ФССП_" + DateTime.Now.ToString("yyyyMMdd_HHmmss_fff", CultureInfo.InvariantCulture);
            string path = Path.Combine(projectDirectory, baseName);
            int suffix = 1;

            while (Directory.Exists(path))
            {
                path = Path.Combine(projectDirectory, baseName + "_" + suffix.ToString(CultureInfo.InvariantCulture));
                suffix++;
            }

            Directory.CreateDirectory(path);
            return path;
        }

        private static string FindProjectDirectory()
        {
            var directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            while (directory != null)
            {
                if (directory.GetFiles("*.csproj").Length > 0)
                {
                    return directory.FullName;
                }

                directory = directory.Parent;
            }

            return AppDomain.CurrentDomain.BaseDirectory;
        }

        private static int ExportFinesByDriver(List<FineExportRecord> fines, string outputDirectory)
        {
            var drivers = fines.GroupBy(fine => fine.DriverId);

            foreach (var driverFines in drivers)
            {
                FineExportRecord driver = driverFines.First();
                string fileName = string.Join("_", new[] { driver.Surname, driver.Name, driver.Patronymic }
                    .Where(part => !string.IsNullOrWhiteSpace(part)));
                fileName = SanitizeFileName(fileName);
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    fileName = "Водитель";
                }

                string filePath = Path.Combine(
                    outputDirectory,
                    fileName + "_" + driver.DriverId.ToString("N").Substring(0, 8) + ".csv");
                var csv = new StringBuilder();
                csv.AppendLine("Фамилия;Имя;Отчество;Паспорт;Город регистрации;Адрес регистрации;" +
                    "Город проживания;Адрес проживания;Госномер;Дата нарушения;Статья;Сумма штрафа;" +
                    "Номер постановления;Вид ДТП");

                foreach (var fine in driverFines)
                {
                    csv.AppendLine(string.Join(";", new[]
                    {
                        EscapeCsvField(fine.Surname),
                        EscapeCsvField(fine.Name),
                        EscapeCsvField(fine.Patronymic),
                        EscapeCsvField(fine.Passport),
                        EscapeCsvField(fine.RegistrationCity),
                        EscapeCsvField(fine.RegistrationAddress),
                        EscapeCsvField(fine.ResidenceCity),
                        EscapeCsvField(fine.ResidenceAddress),
                        EscapeCsvField(fine.VehicleNumber),
                        EscapeCsvField(fine.ViolationDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)),
                        EscapeCsvField(fine.Article),
                        EscapeCsvField(fine.Amount.ToString(CultureInfo.InvariantCulture)),
                        EscapeCsvField(fine.ExternalId),
                        EscapeCsvField(fine.AccidentType)
                    }));
                }

                File.WriteAllText(filePath, csv.ToString(), new UTF8Encoding(true));
            }

            return drivers.Count();
        }

        private static string EscapeCsvField(string value)
        {
            return "\"" + (value ?? string.Empty).Replace("\"", "\"\"") + "\"";
        }

        private static string SanitizeFileName(string fileName)
        {
            foreach (char invalidCharacter in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(invalidCharacter, '_');
            }

            return fileName.Trim().Trim('.');
        }

        private static void MarkFinesAsTransferred(
            SqlConnection connection,
            SqlTransaction transaction,
            List<FineExportRecord> fines)
        {
            const string sql = "UPDATE dbo.Fines SET Status = @TransferredStatus " +
                "WHERE Id = @FineId AND Status = @UnpaidStatus";

            using (var command = new SqlCommand(sql, connection, transaction))
            {
                command.Parameters.Add("@TransferredStatus", SqlDbType.NVarChar, 50).Value = "Передан в ФССП";
                var fineIdParameter = command.Parameters.Add("@FineId", SqlDbType.UniqueIdentifier);
                command.Parameters.Add("@UnpaidStatus", SqlDbType.NVarChar, 50).Value = "Не оплачен";

                foreach (var fine in fines)
                {
                    fineIdParameter.Value = fine.FineId;
                    if (command.ExecuteNonQuery() != 1)
                    {
                        throw new InvalidOperationException(
                            $"Не удалось изменить статус штрафа {fine.FineId}.");
                    }
                }
            }
        }

        
    }
}
