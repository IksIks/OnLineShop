using Npgsql;
using OnLineShop.DBContext;
using OnLineShop.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace OnLineShop.Data
{
    internal class DatabaseProcessing
    {
        private readonly SqlConnection connectionClientsDB;
        private readonly NpgsqlConnection connectionProductDB;
        private readonly NpgsqlConnectionStringBuilder connectionStringProductDB;
        private readonly SqlConnectionStringBuilder connectionStringClientsDB;

        private DataTable ClientsDataTable = new DataTable();
        private DataTable ProductDataTable;

        private SqlDataAdapter SqlDataAdapterClientDB;
        private NpgsqlDataAdapter NpgsqlDataAdapterRoductDB;

        public Func<string, Task<IEnumerable<Client>>> FillClientsDataTable;
        public Func<string, Task<IEnumerable<Client>>> FillProductDataTable;

        private string sqlRequest;

        private ClientsDbContext clientsDB = new();
        private ProductDbContext productDB = new();

        public DatabaseProcessing()
        {
            connectionStringClientsDB = new SqlConnectionStringBuilder()
            {
                DataSource = @"(localdb)\MSSQLLocalDB",
                AttachDBFilename = @"C:\YandexDisk\IKS\C#_проекты\Проекты\OnLineShop\OnLineShop\DB\CLIENTSDB.MDF",
                InitialCatalog = "ClientsDB",
                IntegratedSecurity = true,
                Pooling = true
            };

            connectionStringProductDB = new NpgsqlConnectionStringBuilder()
            {
                Host = "localhost",
                Database = "ProductDB",
                Username = "postgres",
                Password = "1"
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
            NpgsqlDataAdapterRoductDB = new NpgsqlDataAdapter("SELECT * FROM shoppingcart", connectionProductDB);
        }

        //private async Task<DataTable> FillDataTable(string Db)
        //{
        //    if (Db == "ClentsDB")
        //    {
        //        await Task.Run(() => SqlDataAdapterClientDB.Fill(ClientsDataTable));
        //        return ClientsDataTable;
        //    }
        //    else
        //    {
        //        await Task.Run(() => NpgsqlDataAdapterRoductDB.Fill(ProductDataTable = new DataTable()));
        //        return ProductDataTable;
        //    }
        //}

        /// <summary>Запуск соединения с базой</summary>
        /// <returns>состояние соединения</returns>
        public async Task<bool> StartConnectionDBAsync(string s)
        {
            try
            {
                if (s == "ClientsDB")
                {
                    //await Task.Run(() => connectionClientsDB.OpenAsync());
                    FillClientsDataTable = FillDataTable;
                    return await Task.Run(() => clientsDB.Database.CanConnectAsync());
                }
                else
                {
                    //await Task.Run(() => connectionProductDB.OpenAsync());
                    FillProductDataTable = FillDataTable;
                    //return connectionProductDB.State.ToString();
                    return await Task.Run(() => productDB.Database.CanConnectAsync());
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message}");
                return false;
            }
            //finally
            //{
            //    if (s == "ClentsDB")
            //        await Task.Run(() => connectionClientsDB.Close());
            //    await Task.Run(() => connectionProductDB.Close());
            //}
        }

        public async Task<IEnumerable<Client>> FillDataTable(string Db)
        {
            if (Db == "ClientsDB")
            {
                //await Task.Run(() => SqlDataAdapterClientDB.Fill(ClientsDataTable));
                return await Task.Run(() => clientsDB.Clients.ToList());
                //return ClientsDataTable;
            }
            else
            {
                return await Task.Run(() => clientsDB.Clients);
                //await Task.Run(() => NpgsqlDataAdapterRoductDB.Fill(ProductDataTable = new DataTable()));
                //return ProductDataTable;
            }
        }

        /*#region Запросы к БД

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

        public async Task<DataTable> CustomerProductRequest(string email)
        {
            sqlRequest = $"SELECT * FROM shoppingcart WHERE email = '{email}'";
            NpgsqlDataAdapterRoductDB.SelectCommand.CommandText = sqlRequest;
            return await FillDataTable(" ");
        }

        #endregion Запросы к БД*/
    }
}