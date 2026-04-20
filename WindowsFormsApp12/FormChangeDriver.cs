using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace WindowsFormsApp12
{
    public partial class FormChangeDriver : Form
    {
        FormCreateDriver obj = new FormCreateDriver();
        byte[] currentBytes = { };
        public FormChangeDriver()
        {
            InitializeComponent();
        }

        public bool ValidateData()
        {
            bool isValid = true;
            errorProvider1.Clear();

            if (string.IsNullOrWhiteSpace(textBoxSurName.Text))
            {
                errorProvider1.SetError(textBoxSurName, "Введите фамилию");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(textBoxName.Text))
            {
                errorProvider1.SetError(textBoxName, "Введите имя");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(textBoxPatromic.Text))
            {
                errorProvider1.SetError(textBoxPatromic, "Введите отчество");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(textBoxPassport.Text))
            {
                errorProvider1.SetError(textBoxPassport, "Введите серию и номер паспорта");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(textBoxCityRegister.Text))
            {
                errorProvider1.SetError(textBoxCityRegister, "Введите город регистрации");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(textBoxStreetRegister.Text))
            {
                errorProvider1.SetError(textBoxStreetRegister, "Введите улицу регистрации");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(textBoxCityResident.Text))
            {
                errorProvider1.SetError(textBoxCityResident, "Введите город проживания");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(textBoxStreetResident.Text))
            {
                errorProvider1.SetError(textBoxStreetResident, "Введите улицу проживания");
                isValid = false;
            }

            if (!maskedTxtBoxPhoneNumber.MaskCompleted)
            {
                errorProvider1.SetError(maskedTxtBoxPhoneNumber, "Пожалуйста, введите номер телефона полностью");
                maskedTxtBoxPhoneNumber.Focus();
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(textBoxEmail.Text.Trim()))
            {
                errorProvider1.SetError(textBoxEmail, "Email обязателен для заполнения");
                isValid = false;
            }
            else if (!Regex.IsMatch(textBoxEmail.Text.Trim(), @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            {
                errorProvider1.SetError(textBoxEmail, "Введите корректный email (пример: user@example.com)");
                isValid = false;
            }

            if(openFileDialog1.FileName == "photo.png")
            {
                
            }
            else
            {
                if (string.IsNullOrWhiteSpace(obj.selectedFilePath))
                {
                    errorProvider1.SetError(btnChoseImage, "Фото не выбрано");
                    isValid = false;
                }
                else
                {
                    try
                    {
                        if (!File.Exists(obj.selectedFilePath))
                        {
                            errorProvider1.SetError(btnChoseImage, "Выбранный файл не найден");
                            isValid = false;
                        }
                        else
                        {
                            if (pictureBox1.Image == null)
                            {
                                errorProvider1.SetError(btnChoseImage, "Не удалось загрузить превью фото");
                                isValid = false;
                            }

                            long maxFileSize = 2 * 1024 * 1024;
                            long fileSize = new FileInfo(obj.selectedFilePath).Length;

                            if (fileSize > maxFileSize)
                            {
                                errorProvider1.SetError(btnChoseImage, $"Размер фото {obj.FormatFileSize(fileSize)} превышает лимит 2 МБ");
                                isValid = false;
                            }
                            else
                            {
                                // Проверка 3 и 4: Загружаем изображение для проверки размеров
                                using (Image img = Image.FromFile(obj.selectedFilePath))
                                {
                                    int width = img.Width;
                                    int height = img.Height;

                                    // Проверка 3: Изображение должно быть вертикальным (портретная ориентация)
                                    if (width >= height)
                                    {
                                        errorProvider1.SetError(btnChoseImage, "Фото должно быть вертикальным (портретная ориентация)");
                                        isValid = false;
                                    }
                                    else
                                    {
                                        double aspectRatio = (double)width / height;
                                        double targetRatio = 3.0 / 4.0;
                                        double tolerance = 0.05;

                                        double ratioDifference = Math.Abs(aspectRatio - targetRatio) / targetRatio;

                                        if (ratioDifference > tolerance)
                                        {
                                            errorProvider1.SetError(btnChoseImage, $"Соотношение сторон фото должно быть 3:4 (сейчас {width}x{height})");
                                            isValid = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (ArgumentException)
                    {
                        // Файл не является корректным изображением
                        errorProvider1.SetError(btnChoseImage, "Выбранный файл не является изображением");
                        isValid = false;
                    }
                    catch (OutOfMemoryException)
                    {
                        // Изображение повреждено или в неверном формате
                        errorProvider1.SetError(btnChoseImage, "Невозможно прочитать изображение (файл повреждён)");
                        isValid = false;
                    }
                    catch (Exception)
                    {
                        errorProvider1.SetError(btnChoseImage, "Ошибка при проверке фото");
                        isValid = false;
                    }
                }
            }
            

            return isValid;
        }

        public void LoadDriverData(Guid driverId)
        {
            using (var connection = new SqlConnection())
            {
                connection.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;
                 Initial Catalog=gibddDB;
                 Integrated Security=SSPI;";
                connection.Open();

                using (var command = new SqlCommand("SELECT * FROM Clients WHERE Id = @DriverId", connection))
                {
                    command.Parameters.AddWithValue("@DriverId", driverId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            textBoxIDDriver.Text = reader["Id"].ToString();
                            textBoxSurName.Text = reader["SurName"].ToString();
                            textBoxName.Text = reader["Name"].ToString();
                            textBoxPatromic.Text = reader["Patromic"].ToString();
                            textBoxPassport.Text = reader["Passport"].ToString();
                            textBoxCityRegister.Text = reader["Registration_city"].ToString();
                            textBoxStreetRegister.Text = reader["Registration_street_address"].ToString();
                            textBoxCityResident.Text = reader["Residence_city"].ToString();
                            textBoxStreetResident.Text = reader["Residence_street_address"].ToString();
                            textBoxWorkPlace.Text = reader["Place_work"].ToString();
                            textBoxPost.Text = reader["Post"].ToString();
                            maskedTxtBoxPhoneNumber.Text = reader["Mobile_phone"].ToString();
                            textBoxEmail.Text = reader["Email"].ToString();
                            textBoxRemarks.Text = reader["Remarks"].ToString();
                            var photoBytes = (byte[])reader["Photo"];
                            currentBytes = (byte[])reader["Photo"];
                            using (var memoryStream = new MemoryStream(photoBytes))
                            {
                                pictureBox1.Image = Image.FromStream(memoryStream);
                                openFileDialog1.FileName = "photo.png";
                            }
                        }
                    }
                }
            }
        }
        private void FormChangeDriver_Load(object sender, EventArgs e)
        {

        }

        private void btnChangeOkDriver_Click(object sender, EventArgs e)
        {
            if(ValidateData())
            {
                using (var connection = new SqlConnection())
                {
                    connection.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;
                 Initial Catalog=gibddDB;
                 Integrated Security=SSPI;";
                    connection.Open();
                    using (var command = new SqlCommand("UPDATE Clients SET SurName = @SurName, Name = @Name, Patromic = @Patromic, Passport = @Passport, Registration_city = @Registration_city, Registration_street_address = @Registration_street_address, Residence_city = @Residence_city, Residence_street_address = @Residence_street_address, Place_work = @Place_work, Post = @Post, Mobile_phone = @Mobile_phone, Email = @Email, Photo = @Photo, Remarks = @Remarks WHERE Id = @DriverId", connection))
                    {

                        byte[] photo = obj.selectedFilePath == string.Empty ? currentBytes : obj.ImageToByteArray(pictureBox1.Image);
                        command.Parameters.AddWithValue("@DriverId", textBoxIDDriver.Text);
                        command.Parameters.AddWithValue("@SurName", textBoxSurName.Text);
                        command.Parameters.AddWithValue("@Name", textBoxName.Text);
                        command.Parameters.AddWithValue("@Patromic", textBoxPatromic.Text);
                        command.Parameters.AddWithValue("@Passport", textBoxPassport.Text);
                        command.Parameters.AddWithValue("@Registration_city", textBoxCityRegister.Text);
                        command.Parameters.AddWithValue("@Registration_street_address", textBoxStreetRegister.Text);
                        command.Parameters.AddWithValue("@Residence_city", textBoxCityResident.Text);
                        command.Parameters.AddWithValue("@Residence_street_address", textBoxStreetResident.Text);
                        command.Parameters.AddWithValue("@Place_work", textBoxWorkPlace.Text);
                        command.Parameters.AddWithValue("@Post", textBoxPost.Text);
                        command.Parameters.AddWithValue("@Mobile_phone", maskedTxtBoxPhoneNumber.Text);
                        command.Parameters.AddWithValue("@Email", textBoxEmail.Text);
                        command.Parameters.AddWithValue("@Remarks", textBoxRemarks.Text);
                        command.Parameters.Add("@Photo", SqlDbType.VarBinary, -1).Value = (object)photo ?? DBNull.Value;
                        command.ExecuteNonQuery();
                    }
                }

                this.Close();
            }

            
        }

        private void btnChoseImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.FilterIndex = 1;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // 1. Сохраняем путь
                        obj.selectedFilePath = openFileDialog.FileName;

                        // 2. Загружаем изображение БЕЗ блокировки файла
                        using (var fs = new FileStream(obj.selectedFilePath, FileMode.Open, FileAccess.Read))
                        using (var ms = new MemoryStream())
                        {
                            fs.CopyTo(ms);
                            // Создаём новое изображение из памяти
                            var img = Image.FromStream(ms);

                            // Освобождаем старое изображение (если было)
                            if (pictureBox1.Image != null)
                            {
                                pictureBox1.Image.Dispose();
                            }

                            // Присваиваем новое (клон, чтобы не зависеть от потока)
                            pictureBox1.Image = new Bitmap(img);
                        }

                        // 3. Убираем ошибку, если она была
                        errorProvider1.SetError(btnChoseImage, string.Empty);
                    }
                    catch (ArgumentException)
                    {
                        // Файл не является изображением
                        errorProvider1.SetError(btnChoseImage, "Выбранный файл не является изображением");
                        pictureBox1.Image = null;
                        obj.selectedFilePath = string.Empty;
                    }
                    catch (Exception ex)
                    {
                        errorProvider1.SetError(btnChoseImage, $"Ошибка загрузки: {ex.Message}");
                        pictureBox1.Image = null;
                        obj.selectedFilePath = string.Empty;
                    }
                }
            }
        }
    }
}
