using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp12
{
    public partial class Form1 : Form
    {
        private int _remainingSeconds = 0;
        public Form1()
        {
            InitializeComponent();

            timer1.Interval = 1000; // 1 секунда
            timer1.Tick += Timer1_Tick;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckLockoutStatus();
        }

        private void CheckLockoutStatus()
        {
            // Проверяем, есть ли активная блокировка
            if (LockoutManager.IsLocked(out int seconds))
            {
                BlockInterface(true);
                _remainingSeconds = seconds;
                UpdateButtonText();
                timer1.Start();
            }
            else
            {
                BlockInterface(false);
                timer1.Stop();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (LockoutManager.IsLocked(out _))
            {
                return;
            }

            // 2. Имитация проверки пароля
            string login = textBoxLogin.Text;
            string pass = textBoxPassword.Text;

            bool isValid = (login == "inspector" && pass == login); // Пример проверки

            if (isValid)
            {
                // Успешный вход
                LockoutManager.Reset(); // Сбрасываем ошибки
                Form2 nextForm = new Form2();
                nextForm.Show();
                this.Hide();
            }
            else
            {
                // Ошибка входа
                MessageBox.Show("Неверный логин или пароль!");
                LockoutManager.AddFailedAttempt(); // Записываем ошибку в файл

                // Сразу проверяем, не заблокировало ли нас после этой ошибки
                CheckLockoutStatus();
            }
        }
        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (LockoutManager.IsLocked(out int seconds))
            {
                _remainingSeconds = seconds;
                UpdateButtonText();
            }
            else
            {
                button1.Text = "Войти";
                // Сбрасываем счетчик попыток визуально (опционально)
            }
        }
        private void UpdateButtonText()
        {
            // Показываем пользователю сколько ждать
            button1.Text = $"Ждите {_remainingSeconds} сек.";
            button1.Enabled = false;
        }

        private void BlockInterface(bool isBlocked)
        {
            button1.Enabled = !isBlocked;
            textBoxLogin.Enabled = !isBlocked;
            textBoxPassword.Enabled = !isBlocked;

            if (!isBlocked)
            {
                button1.Text = "Войти";
                // Сбрасываем счетчик попыток визуально (опционально)
            }
        }
    }
}
