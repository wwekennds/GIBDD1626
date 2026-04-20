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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace WindowsFormsApp12
{
    public partial class FormCreateDriver : Form
    {
        public string selectedFilePath = string.Empty;
        private ErrorProvider errorProvider = new ErrorProvider();
        Driver driver = new Driver();
        public FormCreateDriver()
        {
            InitializeComponent();
            SetupCityAutoComplete();
            errorProvider.ContainerControl = this;
        }

        private void SetupCityAutoComplete()
        {
            var cities = new[]
            {
                "Архангельск", "Астрахань", "Барнаул", "Белгород", "Брянск",
                "Владивосток", "Владимир", "Волгоград", "Воронеж", "Екатеринбург",
                "Иваново", "Ижевск", "Иркутск", "Казань", "Калининград",
                "Кемерово", "Киров", "Краснодар", "Красноярск", "Курск",
                "Липецк", "Магнитогорск", "Махачкала", "Москва", "Набережные Челны",
                "Нижний Новгород", "Нижний Тагил", "Новокузнецк", "Новосибирск", "Омск",
                "Оренбург", "Пенза", "Пермь", "Ростов-на-Дону", "Рязань",
                "Самара", "Санкт-Петербург", "Саратов", "Саранск", "Ставрополь", "Тверь",
                "Тольятти", "Томск", "Тула", "Тюмень", "Ульяновск",
                "Уфа", "Хабаровск", "Чебоксары", "Челябинск", "Ярославль"
            };
            var collection = new AutoCompleteStringCollection();
            collection.AddRange(cities);

            // Применяем к нужным полям
            ConfigureAutoComplete(textBoxCityResident, collection);
            ConfigureAutoComplete(textBoxCityRegister, collection);
        }

        private void ConfigureAutoComplete(TextBox tb, AutoCompleteStringCollection source)
        {
            tb.AutoCompleteCustomSource = source;
            tb.AutoCompleteMode = AutoCompleteMode.SuggestAppend; 
            tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        public bool ValidateData()
        {
            bool isValid = true;
            errorProvider1.Clear();

            if(string.IsNullOrWhiteSpace(textBoxSurName.Text))
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
            else if(!Regex.IsMatch(textBoxEmail.Text.Trim(), @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            {
                errorProvider1.SetError(textBoxEmail, "Введите корректный email (пример: user@example.com)");
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(selectedFilePath))
            {
                errorProvider1.SetError(btnChoseImage, "Фото не выбрано");
                isValid = false;
            }
            else
            {
                try
                {
                    if (!File.Exists(selectedFilePath))
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
                        long fileSize = new FileInfo(selectedFilePath).Length;

                        if (fileSize > maxFileSize)
                        {
                            errorProvider1.SetError(btnChoseImage, $"Размер фото {FormatFileSize(fileSize)} превышает лимит 2 МБ");
                            isValid = false;
                        }
                        else
                        {
                            // Проверка 3 и 4: Загружаем изображение для проверки размеров
                            using (Image img = Image.FromFile(selectedFilePath))
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

            return isValid;
        }

        public string FormatFileSize(long bytes)
        {
            string[] sizes = { "Б", "КБ", "МБ", "ГБ" };
            int order = 0;
            double size = bytes;

            while (size >= 1024 && order < sizes.Length - 1)
            {
                order++;
                size /= 1024;
            }

            return $"{size:0.##} {sizes[order]}";
        }

        public byte[] ImageToByteArray(Image image)
        {
            if (image == null)
                return null;

            using (MemoryStream ms = new MemoryStream())
            {
                string extensionOfFile = Path.GetExtension(selectedFilePath);
                if(selectedFilePath == ".png")
                {
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                }
                else
                {
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                    
                return ms.ToArray();
            }
        }

        private void btnCreateDriver_Click(object sender, EventArgs e)
        {
           
            if (ValidateData())
            {
                driver.Surname = textBoxSurName.Text;
                driver.Name = textBoxName.Text;
                driver.Patromic = textBoxPatromic.Text;
                driver.Passport = textBoxPassport.Text;
                driver.RegistrationCity = textBoxCityRegister.Text;
                driver.RegistrationStreetAddress = textBoxStreetRegister.Text;
                driver.ResidenceCity = textBoxCityResident.Text;
                driver.ResidenceStreetAddress = textBoxStreetResident.Text;
                driver.PlaceWork = textBoxWorkPlace.Text;
                driver.Post = textBoxPost.Text;
                driver.MobilePhone = maskedTxtBoxPhoneNumber.Text;
                driver.Email = textBoxEmail.Text;
                driver.Photo = ImageToByteArray(pictureBox1.Image);
                driver.Remarks = textBoxRemarks.Text;
                

                using (SqlConnection conn = new SqlConnection())
                {
                    try
                    {
                        conn.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;
                 Initial Catalog=gibddDB;
                 Integrated Security=SSPI;";
                        conn.Open();

                        string query = @"INSERT INTO Clients (SurName, Name, Patromic, Passport, Registration_city, Registration_street_address, Residence_city, Residence_street_address, Place_work, Post, Mobile_phone, Email, Photo, Remarks) OUTPUT INSERTED.Id
                          VALUES (@SurName, @Name, @Patromic, @Passport, @Registration_city, @Registration_street_address, @Residence_city, @Residence_street_address, @Place_work, @Post, @Mobile_phone, @Email, @Photo, @Remarks)";
                        
                        using(SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@SurName", driver.Surname);
                            cmd.Parameters.AddWithValue("@Name", driver.Name);
                            cmd.Parameters.AddWithValue("@Patromic", driver.Patromic);
                            cmd.Parameters.AddWithValue("@Passport", driver.Passport);
                            cmd.Parameters.AddWithValue("@Registration_city", driver.RegistrationCity);
                            cmd.Parameters.AddWithValue("@Registration_street_address", driver.RegistrationStreetAddress);
                            cmd.Parameters.AddWithValue("@Residence_city", driver.ResidenceCity);
                            cmd.Parameters.AddWithValue("@Residence_street_address", driver.ResidenceStreetAddress);
                            cmd.Parameters.AddWithValue("@Place_work", driver.PlaceWork);
                            cmd.Parameters.AddWithValue("@Post", driver.Post);
                            cmd.Parameters.AddWithValue("@Mobile_phone", driver.MobilePhone);
                            cmd.Parameters.AddWithValue("@Email", driver.Email);
                            cmd.Parameters.AddWithValue("@Remarks", driver.Remarks);
                            cmd.Parameters.Add("@Photo", SqlDbType.VarBinary, -1).Value = (object)driver.Photo ?? DBNull.Value;

                            driver.Id = (Guid)cmd.ExecuteScalar();


                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show($"SQL Error: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    finally
                    {
                        MessageBox.Show($"Поздравляем! Водитель успешно создан. Вот его значения: " +
                            $"\nФамилия: {driver.Surname}\nИмя: {driver.Name}\nОтчество: {driver.Patromic}" +
                            $"\nПаспорт: {driver.Passport}\nМесто работы: {driver.PlaceWork}\nДолжность: {driver.Post}" +
                            $"\nМобильный телефон: +7{driver.MobilePhone}\nEmail: {driver.Email}\n\tАдрес проживания:" +
                            $"\nГород: {driver.ResidenceCity}\tУлица, дом: {driver.ResidenceStreetAddress}\n\tАдрес регистрации:" +
                            $"\nГород: {driver.RegistrationCity}\tУлица, дом: {driver.RegistrationStreetAddress}\n" +
                            $"Замечания: {driver.Remarks}\n");

                        //// Очистка текстовых полей
                        textBoxSurName.Clear();
                        textBoxName.Clear();
                        textBoxPatromic.Clear();
                        textBoxWorkPlace.Clear();
                        textBoxPassport.Clear();
                        textBoxPost.Clear();
                        maskedTxtBoxPhoneNumber.Clear();
                        textBoxEmail.Clear();
                        textBoxCityRegister.Clear();
                        textBoxCityResident.Clear();
                        textBoxStreetRegister.Clear();
                        textBoxStreetResident.Clear();
                        textBoxRemarks.Clear();
                        pictureBox1.Image.Dispose(); // Освобождаем ресурсы
                        pictureBox1.Image = null;
                        selectedFilePath = string.Empty;
                        errorProvider1.Clear();
                    }
                }
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
                        selectedFilePath = openFileDialog.FileName;

                        // 2. Загружаем изображение БЕЗ блокировки файла
                        using (var fs = new FileStream(selectedFilePath, FileMode.Open, FileAccess.Read))
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
                        selectedFilePath = string.Empty;
                    }
                    catch (Exception ex)
                    {
                        errorProvider1.SetError(btnChoseImage, $"Ошибка загрузки: {ex.Message}");
                        pictureBox1.Image = null;
                        selectedFilePath = string.Empty;
                    }
                }
            }
        }

        private void FormCreateDriver_Load(object sender, EventArgs e)
        {

        }
    }
}
