using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OnLineShop.Service
{
    internal class DBWork
    {
        private readonly SqlConnection connectionClientsDB, connectionProductDB;
        private readonly SqlConnectionStringBuilder connectionStringClientsDB;


        public DBWork()
        {
            connectionStringClientsDB = new SqlConnectionStringBuilder()
            {
                DataSource = @"(localdb)\MSSQLLocalDB",
                InitialCatalog = "ClientsDB",
                IntegratedSecurity = true,
                Pooling = true
            };

            connectionClientsDB = new SqlConnection()
            {
                ConnectionString = connectionStringClientsDB.ConnectionString
            };
        }

        /// <summary>Запуск соединения с базой</summary>
        /// <returns>состояние соединения</returns>
        private async Task<string> StartConnectionDBAsync()
        {
            try
            {
                if (dbChoise == "0")
                {
                    await Task.Run(() => connectionClientsDB.Open());
                    return connectionClientsDB.State.ToString();
                }
                else
                {
                    await Task.Run(() => connectionProductDB.Open());
                    return connectionProductDB.State.ToString();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message}");
                return "Closed";
            }

        }
}
