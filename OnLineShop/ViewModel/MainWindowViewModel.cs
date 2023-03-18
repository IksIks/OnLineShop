using OnLineShop.Command;
using OnLineShop.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OnLineShop.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private string testConntection;
        private SqlConnection connectionClientsDB;
        private string clienBaseColorStatus, productBaseColorStatus = "Red";


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


        #region Команды

        #region Команда загрузки базы
        public ICommand ConnetcClientDBCommand { get; }
        private bool CanConnetcClientDBCommandExecute(object parameter) => true;
        private void OnConnetcClientDBCommandExecuted(object parameter)
        {
            connectionClientsDB.Open();
            ClienBaseColorStatus = "Green";
            TestConntection = connectionClientsDB.State.ToString();
        }  
        #endregion

        #endregion




        public MainWindowViewModel()
        {

            SqlConnectionStringBuilder connectionStringClientsDB = new SqlConnectionStringBuilder()
            {
                DataSource = @"(localdb)\MSSQLLocalDB",
                InitialCatalog = "ClientsDB",
                IntegratedSecurity = false
            };

            connectionClientsDB = new SqlConnection()
            {
                ConnectionString = connectionStringClientsDB.ConnectionString
            };

            ConnetcClientDBCommand = new LambdaCommand(OnConnetcClientDBCommandExecuted, CanConnetcClientDBCommandExecute);
        }



    }
}
