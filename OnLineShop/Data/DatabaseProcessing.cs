using OnLineShop.DBContext;
using OnLineShop.Model;
using OnLineShop.Model.INPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace OnLineShop.Data
{
    internal class DatabaseProcessing
    {
        private ClientsDbContext clientsDB = new();
        private ProductDbContext productDB = new();

        public async Task<bool> StartConnectionDBAsync(string s)
        {
            try
            {
                if (s == "ClientsDB")
                    using (clientsDB = new ClientsDbContext())
                    {
                        return await Task.Run(() => clientsDB.Database.CanConnectAsync());
                    }
                else
                    using (productDB = new ProductDbContext())
                    {
                        return await Task.Run(() => productDB.Database.CanConnectAsync());
                    }
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message}");
                return false;
            }
        }

        public async Task<IEnumerable<BaseINPC>> GetDataFromDBAsync(string Db)
        {
            try
            {
                if (Db == "ClientsDB")
                    using (clientsDB = new ClientsDbContext())
                    {
                        return await Task.Run(() => clientsDB.Clients.ToList());
                    }
                else
                    using (productDB = new ProductDbContext())
                    {
                        return await Task.Run(() => productDB.Shoppingcarts.ToList());
                    }
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message}");
                return new List<BaseINPC>();
            }
        }

        #region Запросы к БД

        public void InsertNewCustomerRequest(Client newClient)
        {
            try
            {
                using (clientsDB = new ClientsDbContext())
                {
                    clientsDB.Clients.Add(newClient);
                    clientsDB.SaveChanges();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message}");
            }
        }

        public void UpdateCustomerRequest(Client client)
        {
            try
            {
                using (clientsDB = new ClientsDbContext())
                {
                    clientsDB.Clients.Update(client);
                    clientsDB.SaveChanges();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message}");
            }
        }

        public void RemoveCustomerRequest(Client client)
        {
            try
            {
                using (clientsDB = new ClientsDbContext())
                {
                    clientsDB.Clients.Remove(client);
                    clientsDB.SaveChanges();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message}");
            }
        }

        public List<Shoppingcart> ClientProductRequest(string email)
        {
            try
            {
                using (productDB = new ProductDbContext())
                {
                    var clientProduct = productDB.Shoppingcarts;
                    return clientProduct.Where(product => product.Email == email).ToList();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message}");
                return new List<Shoppingcart>();
            }
        }

        #endregion Запросы к БД
    }
}