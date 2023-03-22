using OnLineShop.Command;
using OnLineShop.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace OnLineShop.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private string testConntection;
        private SqlConnection connectionClientsDB;
        private string clienBaseColorStatus = "Red", productBaseColorStatus = "Red";
        private SqlConnectionStringBuilder connectionStringClientsDB;
        private SqlConnection connectionProductDB;
        private string dbChoise;


        public string TestConntection
        {
            get => testConntection;
            set => Set(ref testConntection, value);
        }
        public string ClienBaseColorStatus
        {
            get => clienBaseColorStatus;
            set => Set(ref clienBaseColorStatus, value);
        }
        public string ProductBaseColorStatus
        {
            get => productBaseColorStatus;
            set => Set(ref productBaseColorStatus, value);
        }
        public MainWindowViewModel()
        {

            connectionStringClientsDB = new SqlConnectionStringBuilder()
            {
                DataSource = @"(localdb)\MSSQLLocalDB",
                InitialCatalog = "ClientsDB",
                IntegratedSecurity = true,
                //UserID = "Employee",
                //Password = "1234567890",
                Pooling= true
            };

            connectionClientsDB = new SqlConnection()
            {
                ConnectionString = connectionStringClientsDB.ConnectionString
            };

            ConnectClientDBCommand = new LambdaCommand(OnConnectClientDBCommandExecuted, CanConnectClientDBCommandExecute);
        }


        #region Команды

        #region Команда загрузки базы
        public ICommand ConnectClientDBCommand { get; }
        private bool CanConnectClientDBCommandExecute(object parameter) => true;
        private async void OnConnectClientDBCommandExecuted(object parameter)
        {
            dbChoise = parameter as String;
            string answer = await Task<string>.Factory.StartNew(StartConnection);
            if (Equals(answer, "Open"))
            {
                if (dbChoise == "0")
                    ClienBaseColorStatus = "Green";
                else ProductBaseColorStatus = "Green";
            }
            TestConntection = connectionClientsDB.State.ToString();
        }  
        #endregion

        #endregion
        /// <summary>Запуск соединения с базой</summary>
        /// <returns>состояние соединения</returns>
        private string StartConnection()
        {            
            try
            {
                if (dbChoise == "0")
                    connectionClientsDB.Open();
                else connectionProductDB.Open();
                return "Open";

            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message}");
                return "Closed";
            }
            //finally { connectionClientsDB.Close(); }
            
        }






    }
}
