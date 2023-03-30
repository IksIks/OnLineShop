using OnLineShop.Command;
using OnLineShop.Data;
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
        
        private string clienBaseColorStatus = "Red", productBaseColorStatus = "Red";
        private string dbChoise;
        private DatabaseProcessing dataBaseProcessing;

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
            dataBaseProcessing= new DatabaseProcessing();
            ConnectClientDBCommand = new LambdaCommand(OnConnectClientDBCommandExecuted, CanConnectClientDBCommandExecute);
        }


        #region Команды

        #region Команда загрузки базы

            public ICommand ConnectClientDBCommand { get; }
            private bool CanConnectClientDBCommandExecute(object parameter) => true;
            private async void OnConnectClientDBCommandExecuted(object parameter)
            {
                dbChoise= parameter as string;
                string answer = await dataBaseProcessing.StartConnectionDBAsync(dbChoise);
                if (Equals(answer, "Open"))
                {
                    if (dbChoise == "0")
                        ClienBaseColorStatus = "Green";
                    else ProductBaseColorStatus = "Green";
                }
            }

        #endregion

        #endregion
        
            
     
    }
}
