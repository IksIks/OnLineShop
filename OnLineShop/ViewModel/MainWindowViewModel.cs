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
        private readonly SqlConnectionStringBuilder connectionStringClientsDB;
        private readonly SqlConnection connectionProductDB;
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
            string answer = await StartConnectionDBAsync();
            if (Equals(answer, "Open"))
            {
                if (dbChoise == "0")
                    ClienBaseColorStatus = "Green";
                else ProductBaseColorStatus = "Green";
            }
        }  
        #endregion

        #endregion
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
            //finally { connectionClientsDB.Close(); }
            
        }






    }
}
