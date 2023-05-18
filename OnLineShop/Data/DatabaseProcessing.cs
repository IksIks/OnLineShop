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

        private DataTable ClientsDataTable = new DataTable();
        private DataTable ProductDataTable = new DataTable();

        public Func<string, Task<DataTable>> FillClientsDataTable;
        public Func<string, Task<DataTable>> FillProductDataTable;


        private NpgsqlDataAdapter NpgsqlDataAdapterRoductDB;
        private SqlDataAdapter SqlDataAdapterClientDB;

        
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

            SqlDataAdapterClientDB = new SqlDataAdapter("SELECT * FROM Clients", connectionClientsDB);
            NpgsqlDataAdapterRoductDB = new NpgsqlDataAdapter("SELECT * FROM public.\"ShoppingCart\"", connectionProductDB);
        }
        
        private async Task<DataTable> FillDataTable(string Db)
        {
            if (Db == "0")
            {
                await Task.Run(() => SqlDataAdapterClientDB.Fill(ClientsDataTable));
                return ClientsDataTable;
            }
            else
            {
                await Task.Run(() => NpgsqlDataAdapterRoductDB.Fill(ProductDataTable));
                return ProductDataTable;
            }
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
                    FillClientsDataTable = FillDataTable;
                    return connectionClientsDB.State.ToString();
                }
                else
                {
                    await Task.Run(() => connectionProductDB.OpenAsync());
                    FillProductDataTable = FillDataTable;
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
