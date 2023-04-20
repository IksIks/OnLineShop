using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using System.Data.OleDb;
using Npgsql;

namespace OnLineShop.Data
{
    internal class DatabaseProcessing
    {
        private readonly SqlConnection connectionClientsDB;
        private readonly NpgsqlConnection connectionProductDB;
        private readonly NpgsqlConnectionStringBuilder connectionStringProductDB;
        private readonly SqlConnectionStringBuilder connectionStringClientsDB;

        public DatabaseProcessing()
        {
            connectionStringClientsDB = new SqlConnectionStringBuilder()
            {
                DataSource = @"(localdb)\MSSQLLocalDB",
                AttachDBFilename = @"C:\DROPBOX\IKS\C# ПРОЕКТЫ\ПРОЕКТЫ\ONLINESHOP\ONLINESHOP\CLIENTSDB.MDF",
                InitialCatalog = "ClientsDB",
                IntegratedSecurity = true,
                Pooling = true
            };
            //Host=localhost;Database=Product;Username=postgres;Password=***********;Persist Security Info=True
            connectionStringProductDB = new NpgsqlConnectionStringBuilder()
            {
                Host= "localhost",
                Database = "Product",
                Username = "postgres",
                Password= "1"
            };

            connectionClientsDB = new SqlConnection()
            {
                ConnectionString = connectionStringClientsDB.ConnectionString
            };

            connectionProductDB = new NpgsqlConnection()    
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
