using Npgsql;
using OnLineShop.Model;
using System;
using System.Data;
using System.Data.SqlClient;
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

        private SqlDataAdapter SqlDataAdapterClientDB;
        private NpgsqlDataAdapter NpgsqlDataAdapterRoductDB;

        public Func<string, Task<DataTable>> FillClientsDataTable;
        public Func<string, Task<DataTable>> FillProductDataTable;

        private string sqlRequest;

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
            if (Db == "ClentsDB")
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
                if (s == "ClentsDB")
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
                if (s == "ClentsDB")
                    await Task.Run( () => connectionClientsDB.Close());
                await Task.Run(() => connectionProductDB.Close());
            }
        }

        #region Запросы к БД

        public void InsertNewCustomerRequest(Customer newCustomer)
        {
            DataRow row = ClientsDataTable.NewRow();
            row["Surname"] = newCustomer.Surname;
            row["Name"] = newCustomer.Name;
            row["Patronymic"] = newCustomer.Patronymic;
            row["PhoneNumber"] = newCustomer.PhoneNumber;
            row["Email"] = newCustomer.Email;
            ClientsDataTable.Rows.Add(row);

            sqlRequest = @"INSERT INTO Clients (Surname, Name, Patronymic, PhoneNumber, Email)" +
                                "VALUES (@Surname, @Name, @Patronymic, @PhoneNumber, @Email);" +
                                "SET @ID = @@IDENTITY;";
            SqlDataAdapterClientDB.InsertCommand = new SqlCommand(sqlRequest, connectionClientsDB);

            SqlDataAdapterClientDB.InsertCommand.Parameters.Add("@ID", SqlDbType.Int, 4, "ID").Direction = ParameterDirection.Output;
            SqlDataAdapterClientDB.InsertCommand.Parameters.Add("@Surname", SqlDbType.NVarChar, 20, "Surname");
            SqlDataAdapterClientDB.InsertCommand.Parameters.Add("@Name", SqlDbType.NVarChar, 20, "Name");
            SqlDataAdapterClientDB.InsertCommand.Parameters.Add("@Patronymic", SqlDbType.NVarChar, 20, "Patronymic");
            SqlDataAdapterClientDB.InsertCommand.Parameters.Add("@PhoneNumber", SqlDbType.NVarChar, 20, "PhoneNumber");
            SqlDataAdapterClientDB.InsertCommand.Parameters.Add("@Email", SqlDbType.NVarChar, 20, "Email");
            SqlDataAdapterClientDB.Update(ClientsDataTable);
        }

        public void UpdateCustomerRequest(Customer customer)
        {
            foreach (DataRow row in ClientsDataTable.Rows)
            {
                if (Equals(customer.ID, row["ID"]))
                {
                    row["Surname"] = customer.Surname;
                    row["Name"] = customer.Name;
                    row["Patronymic"] = customer.Patronymic;
                    row["PhoneNumber"] = customer.PhoneNumber;
                    row["Email"] = customer.Email;
                }
            }

            sqlRequest = @"UPDATE Clients 
                         SET Surname = @Surname, Name = @Name, Patronymic = @Patronymic, PhoneNumber = @PhoneNumber, Email = @Email 
                         WHERE ID = @ID";
            SqlDataAdapterClientDB.UpdateCommand = new SqlCommand(sqlRequest, connectionClientsDB);

            SqlDataAdapterClientDB.UpdateCommand.Parameters.Add($"@ID", SqlDbType.Int, 4, "ID").SourceVersion = DataRowVersion.Original;
            SqlDataAdapterClientDB.UpdateCommand.Parameters.Add("@Surname", SqlDbType.NVarChar, 20, "Surname");
            SqlDataAdapterClientDB.UpdateCommand.Parameters.Add("@Name", SqlDbType.NVarChar, 20, "Name");
            SqlDataAdapterClientDB.UpdateCommand.Parameters.Add("@Patronymic", SqlDbType.NVarChar, 20, "Patronymic");
            SqlDataAdapterClientDB.UpdateCommand.Parameters.Add("@PhoneNumber", SqlDbType.NVarChar, 20, "PhoneNumber");
            SqlDataAdapterClientDB.UpdateCommand.Parameters.Add("@Email", SqlDbType.NVarChar, 20, "Email");
            SqlDataAdapterClientDB.Update(ClientsDataTable);
        }

        public void RemoveCustomerRequest(DataRowView row)
        {
            row.Row.Delete();
            sqlRequest = "DELETE FROM Clients WHERE ID = @ID";
            SqlDataAdapterClientDB.DeleteCommand = new SqlCommand(sqlRequest, connectionClientsDB);
            SqlDataAdapterClientDB.DeleteCommand.Parameters.Add("@ID", SqlDbType.Int, 4, "ID");
            SqlDataAdapterClientDB.Update(ClientsDataTable);
        } 
        #endregion


    }
}
