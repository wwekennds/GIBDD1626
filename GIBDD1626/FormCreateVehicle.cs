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

namespace GIBDD1626
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
        private Guid? _editingVehicleId;

        public FormCreateVehicle()
        {
            InitializeComponent();
            ConfigureVehiclesGrid();
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            LoadClients();
            LoadVehicles();
        }

        private void ConfigureVehiclesGrid()
        {
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            btnChangeCar.Enabled = false;
            btnDeleteCar.Enabled = false;
        }

        private void LoadVehicles(Guid? selectedVehicleId = null)
        {
            const string query = @"
                SELECT v.Id, v.ClientId,
                       CONCAT(c.Surname, N' ', c.Name, N' ', c.Patromic) AS Driver,
                       v.Series, v.Number, v.RegionCode, v.Brand, v.Model, v.Vin
                FROM Vehicles AS v
                INNER JOIN Clients AS c ON c.Id = v.ClientId
                ORDER BY c.Surname, c.Name, v.Series, v.Number";

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                {
                    DataTable vehicles = new DataTable();
                    adapter.Fill(vehicles);
                    dataGridView1.DataSource = vehicles;
                }

                dataGridView1.Columns["Id"].Visible = false;
                dataGridView1.Columns["ClientId"].Visible = false;
                dataGridView1.Columns["Driver"].HeaderText = "Владелец";
                dataGridView1.Columns["Series"].HeaderText = "Серия";
                dataGridView1.Columns["Number"].HeaderText = "Номер";
                dataGridView1.Columns["RegionCode"].HeaderText = "Регион";
                dataGridView1.Columns["Brand"].HeaderText = "Марка";
                dataGridView1.Columns["Model"].HeaderText = "Модель";
                dataGridView1.Columns["Vin"].HeaderText = "VIN";

                dataGridView1.ClearSelection();
                dataGridView1.CurrentCell = null;

                if (selectedVehicleId.HasValue)
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if ((Guid)row.Cells["Id"].Value == selectedVehicleId.Value)
                        {
                            row.Selected = true;
                            dataGridView1.CurrentCell = row.Cells["Driver"];
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки автомобилей: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            UpdateSelectionButtons();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            UpdateSelectionButtons();
        }

        private void UpdateSelectionButtons()
        {
            bool hasSelection = dataGridView1.SelectedRows.Count > 0 && !_editingVehicleId.HasValue;
            btnChangeCar.Enabled = hasSelection;
            btnDeleteCar.Enabled = hasSelection;
        }

        private Guid? GetSelectedVehicleId()
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                return null;
            }

            object value = dataGridView1.SelectedRows[0].Cells["Id"].Value;
            return value == null || value == DBNull.Value ? (Guid?)null : (Guid)value;
        }

        private void SetEditingMode(bool isEditing)
        {
            _editingVehicleId = isEditing ? GetSelectedVehicleId() : null;
            dataGridView1.Enabled = !isEditing;
            btnSave.Text = isEditing ? "Сохранить изменения" : "Сохранить";
            UpdateSelectionButtons();
        }

        private void LoadClients()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT Id, Surname, Name, Patromic FROM Clients ORDER BY Surname";
                    var clients = new List<ClientDto>();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            clients.Add(new ClientDto
                            {
                                Id = reader.GetGuid(0),
                                FullName = $"{reader.GetString(1)} {reader.GetString(2)} {reader.GetString(3)}"
                            });
                        }
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
            if (char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
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
                    LoadVehicles();
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
            if (!ValidateInput())
            {
                return;
            }

            if (_editingVehicleId.HasValue)
            {
                UpdateVehicle(_editingVehicleId.Value);
            }
            else
            {
                SaveToDatabase();
            }
        }

        private void UpdateVehicle(Guid vehicleId)
        {
            Guid clientId = ((ClientDto)cmbClients.SelectedItem).Id;
            string query = @"UPDATE Vehicles
                             SET ClientId = @ClientId, Series = @Series, Number = @Number,
                                 RegionCode = @RegionCode, Brand = @Brand, Model = @Model, Vin = @Vin
                             WHERE Id = @Id";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Id", vehicleId);
                cmd.Parameters.AddWithValue("@ClientId", clientId);
                cmd.Parameters.AddWithValue("@Series", txtSeries.Text.Trim().ToUpper());
                cmd.Parameters.AddWithValue("@Number", txtNumber.Text.Trim());
                cmd.Parameters.AddWithValue("@RegionCode", txtRegion.Text.Trim());
                cmd.Parameters.AddWithValue("@Brand", string.IsNullOrWhiteSpace(txtBrand.Text) ? (object)DBNull.Value : txtBrand.Text.Trim());
                cmd.Parameters.AddWithValue("@Model", string.IsNullOrWhiteSpace(txtModel.Text) ? (object)DBNull.Value : txtModel.Text.Trim());
                cmd.Parameters.AddWithValue("@Vin", string.IsNullOrWhiteSpace(txtVin.Text) ? (object)DBNull.Value : txtVin.Text.Trim());

                try
                {
                    conn.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        MessageBox.Show("Автомобиль не найден. Обновите список и попробуйте снова.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        SetEditingMode(false);
                        ClearFields();
                        LoadVehicles();
                        return;
                    }

                    MessageBox.Show("Изменения сохранены.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SetEditingMode(false);
                    ClearFields();
                    LoadVehicles(vehicleId);
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

        private void txtRegion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnChangeCar_Click(object sender, EventArgs e)
        {
            Guid? vehicleId = GetSelectedVehicleId();
            if (!vehicleId.HasValue)
            {
                return;
            }

            DataGridViewRow row = dataGridView1.SelectedRows[0];
            cmbClients.SelectedValue = row.Cells["ClientId"].Value;
            txtSeries.Text = Convert.ToString(row.Cells["Series"].Value);
            txtNumber.Text = Convert.ToString(row.Cells["Number"].Value);
            txtRegion.Text = Convert.ToString(row.Cells["RegionCode"].Value);
            txtBrand.Text = Convert.ToString(row.Cells["Brand"].Value);
            txtModel.Text = Convert.ToString(row.Cells["Model"].Value);
            txtVin.Text = Convert.ToString(row.Cells["Vin"].Value);
            errorProvider1.Clear();
            SetEditingMode(true);
        }

        private void btnDeleteCar_Click(object sender, EventArgs e)
        {
            Guid? vehicleId = GetSelectedVehicleId();
            if (!vehicleId.HasValue)
            {
                return;
            }

            if (MessageBox.Show("Удалить выбранный автомобиль?", "Подтверждение удаления",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Vehicles WHERE Id = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", vehicleId.Value);
                    conn.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        MessageBox.Show("Автомобиль не найден. Обновите список и попробуйте снова.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                ClearFields();
                LoadVehicles();
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Не удалось удалить автомобиль: {sqlEx.Message}", "Ошибка SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
