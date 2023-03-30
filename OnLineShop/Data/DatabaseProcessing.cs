using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Data.OleDb;

namespace OnLineShop.Data
{
    internal class DatabaseProcessing
    {
        private readonly SqlConnection connectionClientsDB;
        private readonly OleDbConnection connectionProductDB;
        private readonly OleDbConnectionStringBuilder connectionStringProductDB;
        private readonly SqlConnectionStringBuilder connectionStringClientsDB;



        public DatabaseProcessing()
        {
            connectionStringClientsDB = new SqlConnectionStringBuilder()
            {
                DataSource = @"(localdb)\MSSQLLocalDB",
                AttachDBFilename = @"F:\C# проекты\Проекты\OnLineShop\OnLineShop\ClientsDB.mdf",
                InitialCatalog = "ClientsDB",
                IntegratedSecurity = true,
                Pooling = true
            };

            connectionStringProductDB = new OleDbConnectionStringBuilder()
            {
                DataSource = "ProductDB",
                Provider = "SQLOLEDB",
            };

            connectionClientsDB = new SqlConnection()
            {
                ConnectionString = connectionStringClientsDB.ConnectionString
            };

            connectionProductDB = new OleDbConnection()
            {
                ConnectionString = connectionStringProductDB.ConnectionString
            };
        }

        /// <summary>Запуск соединения с базой</summary>
        /// <returns>состояние соединения</returns>
        public async Task<string> StartConnectionDBAsync(string s)
        {
            try
            {
                if (s == "0")
                {
                    await Task.Run(() => connectionClientsDB.OpenAsync());
                    return connectionClientsDB.State.ToString();
                }
                else
                {
                    await Task.Run(() => connectionProductDB.OpenAsync());
                    return connectionProductDB.State.ToString();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message}");
                return "Closed";
            }
            finally
            {
                if (s == "0")
                    await Task.Run( () => connectionClientsDB.Close());
                await Task.Run(() => connectionProductDB.Close());
            }
        }
    }
}
