using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GIBDD1626
{
    public partial class FinesForm : Form
    {
        private readonly string _connectionString = @"Data Source=(localdb)\MSSQLLocalDB;
                 Initial Catalog=gibddDB;
                 Integrated Security=SSPI;";

        private List<ViolationDto> _violations = new List<ViolationDto>();

        // Список видов ДТП согласно вашему заданию
        private readonly List<string> _accidentTypes = new List<string>
        {
            "Столкновение",
            "Опрокидывание",
            "Наезд на стоящее транспортное средство",
            "Наезд на препятствие",
            "Наезд на пешехода",
            "Наезд на велосипедиста",
            "Наезд на животное",
            "Иные виды ДТП"
        };

        private readonly HashSet<char> _allowedLetters = new HashSet<char>
        { 'А', 'В', 'Е', 'К', 'М', 'Н', 'О', 'Р', 'С', 'Т', 'У', 'Х' };

        public FinesForm()
        {
            InitializeComponent();
            SetupGrid();
            SetupAccidentTypes();
        }

        private void SetupAccidentTypes()
        {
            cmbAccidentType.DataSource = _accidentTypes;
        }

        private void SetupGrid()
        {
            dgvFines.AutoGenerateColumns = false;
            dgvFines.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvFines.MultiSelect = false;

            dgvFines.Columns.Add(new DataGridViewTextBoxColumn { Name = "colId", HeaderText = "ID", DataPropertyName = "ExternalId", Width = 50 });
            dgvFines.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDate", HeaderText = "Дата", DataPropertyName = "ViolationDate", Width = 100 });
            dgvFines.Columns.Add(new DataGridViewTextBoxColumn { Name = "colType", HeaderText = "Тип ДТП (из JSON)", DataPropertyName = "AccidentTypeRaw", Width = 150 });
            dgvFines.Columns.Add(new DataGridViewTextBoxColumn { Name = "colNumber", HeaderText = "Номер", DataPropertyName = "VehicleNumber", Width = 100 });
            dgvFines.Columns.Add(new DataGridViewTextBoxColumn { Name = "colStatus", HeaderText = "Статус", DataPropertyName = "Status", Width = 100 });
        }

        private void FinesForm_Load(object sender, EventArgs e)
        {

        }

        private void btnLoadJson_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "JSON Files|*.json";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string json = File.ReadAllText(ofd.FileName);
                        _violations = JsonConvert.DeserializeObject<List<ViolationDto>>(json);

                        foreach (var v in _violations)
                        {
                            v.IsProcessed = false;
                            v.Status = "Новый";
                            v.SelectedAccidentType = "Иные виды ДТП";
                        }

                        dgvFines.DataSource = new BindingSource(_violations, null);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка JSON: {ex.Message}");
                    }
                }
            }
        }

        private void dgvFines_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvFines.SelectedRows.Count == 0) return;

            var violation = (ViolationDto)dgvFines.SelectedRows[0].DataBoundItem;
            errorProvider1.Clear();
          
            label1.Text = $"Статья: {violation.Article}\nСумма: {violation.Amount} руб.\nДата: {violation.ViolationDate}";

            txtManualNumber.Text = violation.VehicleNumber ?? "";
         
            if (!string.IsNullOrEmpty(violation.AccidentTypeRaw) &&
                _accidentTypes.Contains(violation.AccidentTypeRaw))
            {
                cmbAccidentType.SelectedItem = violation.AccidentTypeRaw;
            }
            else
            {
               
                cmbAccidentType.SelectedItem = "Иные виды ДТП";
            }
           
            violation.SelectedAccidentType = cmbAccidentType.SelectedItem.ToString();
        }

        private void cmbAccidentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvFines.SelectedRows.Count > 0)
            {
                var violation = (ViolationDto)dgvFines.SelectedRows[0].DataBoundItem;
                violation.SelectedAccidentType = cmbAccidentType.SelectedItem.ToString();
            }
        }

        private bool ValidateNumber(string number, out string series, out string digits, out string region)
        {
            series = null;
            digits = null;
            region = null;
            errorProvider1.Clear();

            if (string.IsNullOrWhiteSpace(number))
            {
                errorProvider1.SetError(txtManualNumber, "Введите номер ТС");
                return false;
            }

            string clean = number.ToUpper().Replace(" ", "").Replace("-", "");

            var match = Regex.Match(clean, @"^([АВЕКМНОРСТУХ])(\d{3})([АВЕКМНОРСТУХ]{2})(\d{2,3})$");

            if (!match.Success)
            {
                errorProvider1.SetError(txtManualNumber, "Неверный формат. Пример: А123ВЕ777. Только буквы А,В,Е,К,М,Н,О,Р,С,Т,У,Х");
                return false;
            }

           
            string letter1 = match.Groups[1].Value; 
            digits = match.Groups[2].Value;        
            string letters23 = match.Groups[3].Value; 
            region = match.Groups[4].Value;         

            series = letter1 + letters23;

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvFines.SelectedRows.Count == 0) return;
            var violation = (ViolationDto)dgvFines.SelectedRows[0].DataBoundItem;

            
            if (!ValidateNumber(txtManualNumber.Text, out string series, out string digits, out string region))
                return;

            
            Guid? vehicleId = FindVehicleInDb(series, digits, region);
            if (!vehicleId.HasValue)
            {
                MessageBox.Show("ТС не найдено в базе. Проверьте номер или добавьте авто.");
                violation.Status = "ТС не найдено";
                dgvFines.Refresh();
                return;
            }

           
            SaveFine(violation, vehicleId.Value);

            violation.Status = "Сохранено";
            violation.IsProcessed = true;
            violation.VehicleNumber = $"{series}{digits}{region}"; 
            dgvFines.Refresh();

            SelectNextUnprocessed();
        }

        private void btnSkip_Click(object sender, EventArgs e)
        {
            if (dgvFines.SelectedRows.Count == 0) return;
            var violation = (ViolationDto)dgvFines.SelectedRows[0].DataBoundItem;

            violation.Status = "Запрос на сервер";
            violation.IsProcessed = true;
            dgvFines.Refresh();
            SelectNextUnprocessed();
        }

        private Guid? FindVehicleInDb(string series, string number, string region)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT Id FROM Vehicles WHERE Series = @s AND Number = @n AND RegionCode = @r";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@s", series);
                    cmd.Parameters.AddWithValue("@n", number);
                    cmd.Parameters.AddWithValue("@r", region);
                    var res = cmd.ExecuteScalar();
                    return res != null ? (Guid?)res : null;
                }
            }
        }

        private void SaveFine(ViolationDto v, Guid vehicleId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = @"INSERT INTO Fines (Id, VehicleId, ViolationDate, ArticleCode, FineAmount, ExternalId, AccidentType) 
                               VALUES (NEWID(), @vid, @date, @art, @amt, @eid, @atype)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@vid", vehicleId);
                    cmd.Parameters.AddWithValue("@date", v.ViolationDate);
                    cmd.Parameters.AddWithValue("@art", v.Article);
                    cmd.Parameters.AddWithValue("@amt", v.Amount);
                    cmd.Parameters.AddWithValue("@eid", v.ExternalId);
                    cmd.Parameters.AddWithValue("@atype", v.SelectedAccidentType);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        private void SelectNextUnprocessed()
        {
            foreach (DataGridViewRow row in dgvFines.Rows)
            {
                var v = row.DataBoundItem as ViolationDto;
                if (v != null && !v.IsProcessed)
                {
                    row.Selected = true;
                    dgvFines.FirstDisplayedScrollingRowIndex = row.Index;
                    return;
                }
            }

            dgvFines.ClearSelection();
            label1.Text = "Все штрафы обработаны.";
            txtManualNumber.Clear();
        }

        
    }
}
