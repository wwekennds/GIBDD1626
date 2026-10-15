using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp12
{
    public partial class FormCardsDriver : Form
    {
        public FormCardsDriver()
        {
            InitializeComponent();
        }

        private void FormCardsDriver_Load(object sender, EventArgs e)
        {
            this.clientsTableAdapter.Fill(this.gibddDBDataSet.Clients);

        }
        

        private void btnChange_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Выберите запись!");
                return;
            }

            var row = dataGridView1.Rows[dataGridView1.CurrentRow.Index];
            var driverId = (Guid)row.Cells["idDataGridViewTextBoxColumn"].Value;

            FormChangeDriver formChangeDrivers = new FormChangeDriver();
            formChangeDrivers.LoadDriverData(driverId);
            formChangeDrivers.Show();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var result = MessageBox.Show(
                    "Вы действительно хотите удалить водителя, его автомобили и связанные с ними штрафы?",
                    "Удаление водителя",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    DeleteDriver();
                }
            }
        }

        //private void LoadDriverData()
        //{
        //    using (var connection = new SqlConnection())
        //    {
        //        connection.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;
        // Initial Catalog=gibddDB;
        // Integrated Security=SSPI;";
        //        connection.Open();

        //        using (var command = new SqlCommand("SELECT Id, SurName, Name, Patromic, Passport, Registration_city, Registration_street_address, Residence_city, Residence_street_address, Place_work, Post, Mobile_Phone, Email, Remarks, Photo FROM Clients", connection))
        //        {
        //            using (var reader = command.ExecuteReader())
        //            {
        //                gibddDBDataSet.Clients.Clear();
        //                while (reader.Read())
        //                {
        //                    var row = gibddDBDataSet.Clients.Rows.Count;
        //                    gibddDBDataSet.Clients.Rows.Add(row,
        //                        reader["Id"],
        //                        reader["SurName"],
        //                        reader["Name"],
        //                        reader["Patromic"],
        //                        reader["Passport"],
        //                        reader["Registration_city"],
        //                        reader["Registration_street_address"],
        //                        reader["Residence_city"],
        //                        reader["Residence_street_address"],
        //                        reader["Place_work"],
        //                        reader["Post"],
        //                        reader["Mobile_Phone"],
        //                        reader["Email"],
        //                        reader["Remarks"],
        //                        reader["Photo"]);
        //                }
        //                dataGridView1.DataSource = gibddDBDataSet.Clients;
        //            }
        //        }
        //    }
        //}


        private void DeleteDriver()
        {
            var driverId = (Guid)dataGridView1.SelectedRows[0].Cells["idDataGridViewTextBoxColumn"].Value;

            using (var connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;
         Initial Catalog=gibddDB;
         Integrated Security=SSPI;"))
            {
                try
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        using (var command = new SqlCommand(
                            "DELETE FROM Fines WHERE VehicleId IN (SELECT Id FROM Vehicles WHERE ClientId = @DriverId)",
                            connection,
                            transaction))
                        {
                            command.Parameters.Add("@DriverId", SqlDbType.UniqueIdentifier).Value = driverId;
                            command.ExecuteNonQuery();
                        }

                        using (var command = new SqlCommand(
                            "DELETE FROM Vehicles WHERE ClientId = @DriverId",
                            connection,
                            transaction))
                        {
                            command.Parameters.Add("@DriverId", SqlDbType.UniqueIdentifier).Value = driverId;
                            command.ExecuteNonQuery();
                        }

                        using (var command = new SqlCommand(
                            "DELETE FROM Clients WHERE Id = @DriverId",
                            connection,
                            transaction))
                        {
                            command.Parameters.Add("@DriverId", SqlDbType.UniqueIdentifier).Value = driverId;
                            if (command.ExecuteNonQuery() != 1)
                            {
                                transaction.Rollback();
                                MessageBox.Show(
                                    "Водитель не найден. Возможно, запись уже была удалена.",
                                    "Удаление невозможно",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                                return;
                            }
                        }

                        transaction.Commit();
                    }
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 547)
                    {
                        MessageBox.Show(
                            "Нельзя удалить водителя, пока с его автомобилями связаны записи в других таблицах.",
                            "Удаление невозможно",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show($"Не удалось удалить водителя: {ex.Message}", "Ошибка базы данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    return;
                }
            }

            MessageBox.Show("Водитель успешно удален.");
            this.clientsTableAdapter.Fill(this.gibddDBDataSet.Clients);
        }

        
    }
}
