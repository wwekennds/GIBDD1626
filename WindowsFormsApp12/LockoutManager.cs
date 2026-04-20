using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp12
{
    public static class LockoutManager
    {
        // Путь к файлу настроек (в папке пользователя, чтобы не было проблем с правами доступа)
        private static readonly string FilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "MyAppLockout.dat");

        private const int MaxAttempts = 3;
        private const int LockoutMinutes = 1;

        // Загрузка состояния
        public static void LoadState(out int attempts, out DateTime? lockoutUntil)
        {
            attempts = 0;
            lockoutUntil = null;

            if (File.Exists(FilePath))
            {
                try
                {
                    string[] lines = File.ReadAllLines(FilePath);
                    if (lines.Length >= 1) int.TryParse(lines[0], out attempts);
                    if (lines.Length >= 2)
                    {
                        long ticks;
                        if (long.TryParse(lines[1], out ticks))
                        {
                            lockoutUntil = new DateTime(ticks);
                        }
                    }
                }
                catch
                {
                    // Если файл поврежден, игнорируем ошибку
                }
            }
        }

        // Сохранение состояния
        public static void SaveState(int attempts, DateTime? lockoutUntil)
        {
            try
            {
                string dir = Path.GetDirectoryName(FilePath);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                string line1 = attempts.ToString();
                string line2 = lockoutUntil.HasValue ? lockoutUntil.Value.Ticks.ToString() : "";

                File.WriteAllLines(FilePath, new[] { line1, line2 });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }

        // Сброс при успешном входе
        public static void Reset()
        {
            SaveState(0, null);
        }

        // Проверка, заблокирован ли вход сейчас
        public static bool IsLocked(out int remainingSeconds)
        {
            LoadState(out _, out DateTime? lockoutUntil);
            remainingSeconds = 0;

            if (lockoutUntil.HasValue && DateTime.Now < lockoutUntil.Value)
            {
                TimeSpan diff = lockoutUntil.Value - DateTime.Now;
                remainingSeconds = (int)diff.TotalSeconds;
                return true;
            }

            // Если время вышло, но файл еще есть — очищаем его
            if (lockoutUntil.HasValue && DateTime.Now >= lockoutUntil.Value)
            {
                Reset();
            }

            return false;
        }

        // Увеличение счетчика ошибок
        public static void AddFailedAttempt()
        {
            LoadState(out int attempts, out DateTime? lockoutUntil);

            // Если уже есть активная блокировка, не увеличиваем счетчик
            if (lockoutUntil.HasValue && DateTime.Now < lockoutUntil.Value) return;

            attempts++;

            if (attempts >= MaxAttempts)
            {
                // Блокируем на 1 минуту
                lockoutUntil = DateTime.Now.AddMinutes(LockoutMinutes);
                attempts = 0; // Сбрасываем счетчик, так как теперь действует таймер
            }

            SaveState(attempts, lockoutUntil);
        }
    }
}
