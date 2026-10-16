using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace GIBDD1626
{
    internal static class DatabaseInitializer
    {
        public static void Initialize()
        {
            var databaseConnectionString = Properties.Settings.Default.gibddDBConnectionString;
            var databaseConnection = new SqlConnectionStringBuilder(databaseConnectionString);
            if (string.IsNullOrWhiteSpace(databaseConnection.InitialCatalog))
            {
                throw new InvalidOperationException("В строке подключения не указано имя базы данных.");
            }

            var masterConnection = new SqlConnectionStringBuilder(databaseConnectionString)
            {
                InitialCatalog = "master"
            };

            using (var connection = new SqlConnection(masterConnection.ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT DB_ID(@DatabaseName)";
                    command.Parameters.Add("@DatabaseName", SqlDbType.NVarChar, 128).Value = databaseConnection.InitialCatalog;
                    if (command.ExecuteScalar() == DBNull.Value)
                    {
                        command.Parameters.Clear();
                        var quotedDatabaseName = "[" + databaseConnection.InitialCatalog.Replace("]", "]]") + "]";
                        command.CommandText = "CREATE DATABASE " + quotedDatabaseName;
                        command.ExecuteNonQuery();
                    }
                }
            }

            ExecuteMigrationFile(databaseConnection.ConnectionString, "DatabaseSchema.sql");
            ExecuteMigrationFile(databaseConnection.ConnectionString, "DatabaseSeed.sql");
        }

        private static void ExecuteMigrationFile(string connectionString, string fileName)
        {
            var applicationDirectory = Path.GetDirectoryName(typeof(DatabaseInitializer).Assembly.Location);
            var filePath = Path.Combine(applicationDirectory, fileName);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Не найден файл инициализации базы данных.", filePath);
            }

            var sql = File.ReadAllText(filePath, Encoding.UTF8);
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                using (var command = new SqlCommand(sql, connection, transaction))
                {
                    command.CommandTimeout = 120;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
            }
        }
    }
}
