using Npgsql;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace OnLineShop.Data
{
    internal class DatabaseProcessing
    {
        private readonly SqlConnection connectionClientsDB;
        private readonly NpgsqlConnection connectionProductDB;
        private readonly NpgsqlConnectionStringBuilder connectionStringProductDB;
        private readonly SqlConnectionStringBuilder connectionStringClientsDB;
        public Func<DataTable> FillClientsDataTable;
        public Func<DataTable> FillProductDataTable;
        public DataTable ClientsDataTable = new DataTable();
        public DataTable ProductDataTable = new DataTable();
        public SqlDataAdapter SqlDataAdapterRoductDB = new SqlDataAdapter();
        private string test = "SELECT * FROM ClientsDB.dbo.Clients";
        public SqlDataAdapter SqlDataAdapterClientDB;

        public DatabaseProcessing()
        {
            connectionStringClientsDB = new SqlConnectionStringBuilder()
            {
                DataSource = @"(localdb)\MSSQLLocalDB",
                //AttachDBFilename = @"C:\DROPBOX\IKS\C# ПРОЕКТЫ\ПРОЕКТЫ\ONLINESHOP\ONLINESHOP\DB\CLIENTSDB.MDF",
                InitialCatalog = "ClientsDB",
                IntegratedSecurity = true,
                Pooling = true
            };
            
            connectionStringProductDB = new NpgsqlConnectionStringBuilder()
            {
                Host= "localhost",
                Database = "ProductDB",
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

            SqlDataAdapterClientDB = new SqlDataAdapter(test, connectionClientsDB);
        }

        private DataTable Filling()
        {
            SqlDataAdapterClientDB.Fill(ClientsDataTable);
            return ClientsDataTable;
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
                    FillClientsDataTable = Filling;
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
