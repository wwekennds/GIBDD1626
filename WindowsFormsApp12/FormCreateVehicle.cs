using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WindowsFormsApp12
{
    public partial class FormCreateVehicle : Form
    {
        private class ClientDto
        {
            public Guid Id { get; set; }
            public string FullName { get; set; }
        }

        private readonly string _connectionString = @"Data Source=(localdb)\MSSQLLocalDB;
                 Initial Catalog=gibddDB;
                 Integrated Security=SSPI;";

        private readonly char[] _allowedLetters = { 'А', 'В', 'Е', 'К', 'М', 'Н', 'О', 'Р', 'С', 'Т', 'У', 'Х' };
        public FormCreateVehicle()
        {
            InitializeComponent();
            LoadClients();
        }

        private void LoadClients()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT Id, Surname, Name, Patromic FROM Clients ORDER BY Surname";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    var clients = new List<ClientDto>();
                    while (reader.Read())
                    {
                        clients.Add(new ClientDto
                        {
                            Id = reader.GetGuid(0),
                            FullName = $"{reader.GetString(1)} {reader.GetString(2)} {reader.GetString(3)}"
                        });
                    }

                    cmbClients.DataSource = clients;
                    cmbClients.DisplayMember = "FullName";
                    cmbClients.ValueMember = "Id";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки клиентов: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void FormCreateVehicle_Load(object sender, EventArgs e)
        {

        }

        private void txtSeries_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) && char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private bool ValidateInput()
        {
            bool isValid = true;
            errorProvider1.Clear();

            if (cmbClients.SelectedItem == null)
            {
                errorProvider1.SetError(cmbClients, "Выберите владельца автомобиля");
                isValid = false;
            }

            string series = txtSeries.Text.Trim().ToUpper();
            if (string.IsNullOrEmpty(series) || series.Length != 3)
            {
                errorProvider1.SetError(txtSeries, "Серия должна состоять из 3 букв");
                isValid = false;
            }
            else
            {
                foreach (char c in series)
                {
                    if (Array.IndexOf(_allowedLetters, c) == -1)
                    {
                        errorProvider1.SetError(txtSeries, $"Буква '{c}' недопустима. Разрешены: А, В, Е, К, М, Н, О, Р, С, Т, У, Х");
                        isValid = false;
                        break;
                    }
                }
            }
          
            string number = txtNumber.Text.Trim();
            if (string.IsNullOrEmpty(number) || !Regex.IsMatch(number, @"^\d{3}$"))
            {
                errorProvider1.SetError(txtNumber, "Номер должен состоять из 3 цифр");
                isValid = false;
            }

        
            string region = txtRegion.Text.Trim();
            if (string.IsNullOrEmpty(region) || !Regex.IsMatch(region, @"^\d{2,3}$"))
            {
                errorProvider1.SetError(txtRegion, "Код региона: 2 или 3 цифры");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(txtBrand.Text))
            {
                errorProvider1.SetError(txtBrand, "Укажите марку автомобиля");
                isValid = false;
            }

            return isValid;
        }

        private void SaveToDatabase()
        {
            Guid clientId = ((ClientDto)cmbClients.SelectedItem).Id;
            string series = txtSeries.Text.Trim().ToUpper();
            string number = txtNumber.Text.Trim();
            string region = txtRegion.Text.Trim();
            string brand = txtBrand.Text.Trim();
            string model = txtModel.Text.Trim();
            string vin = txtVin.Text.Trim();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"INSERT INTO Vehicles 
                                   (ClientId, Series, Number, RegionCode, Country, Brand, Model, Vin) 
                                   VALUES 
                                   (@ClientId, @Series, @Number, @RegionCode, @Country, @Brand, @Model, @Vin)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ClientId", clientId);
                        cmd.Parameters.AddWithValue("@Series", series);
                        cmd.Parameters.AddWithValue("@Number", number);
                        cmd.Parameters.AddWithValue("@RegionCode", region);
                        cmd.Parameters.AddWithValue("@Country", "RUS"); 
                        cmd.Parameters.AddWithValue("@Brand", string.IsNullOrEmpty(brand) ? (object)DBNull.Value : brand);
                        cmd.Parameters.AddWithValue("@Model", string.IsNullOrEmpty(model) ? (object)DBNull.Value : model);
                        cmd.Parameters.AddWithValue("@Vin", string.IsNullOrEmpty(vin) ? (object)DBNull.Value : vin);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Транспортное средство успешно добавлено!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                }
                catch (SqlException sqlEx)
                {
                    MessageBox.Show($"Ошибка базы данных: {sqlEx.Message}", "Ошибка SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ClearFields()
        {
            txtSeries.Clear();
            txtNumber.Clear();
            txtRegion.Clear();
            txtBrand.Clear();
            txtModel.Clear();
            txtVin.Clear();
            errorProvider1.Clear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                SaveToDatabase();
            }
        }

        private void txtRegion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
