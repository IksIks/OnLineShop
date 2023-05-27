using OnLineShop.Command;
using OnLineShop.Data;
using OnLineShop.View;
using OnLineShop.ViewModel.Base;
using System;
using System.Data;
using System.Windows.Input;

namespace OnLineShop.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private string testConntection;
        
        private string clienDBColorStatus = "Red", productDBColorStatus = "Red";
        private string dbChoise;
        private DatabaseProcessing dataBaseProcessing;
        
        private DataTable clientsDataGridItemTable;
        private DataTable productDataGridItemTable;
        
        public static event Action<DataRow> ChangeCustomerEvent;

        

        public DataTable ProductDataGridItemTable
        {
            get => productDataGridItemTable;
            set => Set(ref productDataGridItemTable, value);
        }

        public DataTable ClientsDataGridItemTable
        {
            get => clientsDataGridItemTable;
            set => Set(ref clientsDataGridItemTable, value);
        }
        public string TestConntection
        {
            get => testConntection;
            set => Set(ref testConntection, value);
        }
        public string ClienDBColorStatus
        {
            get => clienDBColorStatus;
            set => Set(ref clienDBColorStatus, value);
        }
        public string ProductDBColorStatus
        {
            get => productDBColorStatus;
            set => Set(ref productDBColorStatus, value);
        }

        public MainWindowViewModel()
        {
            dataBaseProcessing= new DatabaseProcessing();
            ConnectClientDBCommand = new LambdaCommand(OnConnectClientDBCommandExecuted, CanConnectClientDBCommandExecute);
            AddClientCommand = new LambdaCommand(OnAddClientCommandExecuted, CanAddClientCommandExecute);
            UpdateCustomerDataCommand = new LambdaCommand(OnUpdateCustomerDataCommandExecuted, CanUpdateCustomerDataCommandExecute);
            RemoveClientCommand = new LambdaCommand(OnRemoveClientCommandExecuted, CanRemoveClientCommandExecute);
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
                    if (dbChoise == "ClentsDB")
                    {
                        ClienDBColorStatus = "Green";
                        ClientsDataGridItemTable = await dataBaseProcessing.FillClientsDataTable(dbChoise);
                    }
                    else
                    {
                        ProductDBColorStatus = "Green";
                        ProductDataGridItemTable = await dataBaseProcessing.FillProductDataTable(dbChoise);
                    }
                }
            }

        #endregion

        #region Команда добавления клиента
        public ICommand AddClientCommand { get; }
        private bool CanAddClientCommandExecute(object parametr) => true;
        private void OnAddClientCommandExecuted(object parameter)
        {
            AddClientViewModel.AddNewCustomer += dataBaseProcessing.InsertNewCustomerRequest;
            AddClient addClient = new AddClient();
            addClient.ShowDialog();
            AddClientViewModel.AddNewCustomer -= dataBaseProcessing.InsertNewCustomerRequest;
        }
        #endregion

        #region Команда обновления данных о клиенте
        public ICommand UpdateCustomerDataCommand { get; }
        private bool CanUpdateCustomerDataCommandExecute(object parameter)
        {
            return true;
        }
        private void OnUpdateCustomerDataCommandExecuted(object parameter)
        {

            var row = (parameter as DataRowView).Row;
            ChangeCustomer changeCustomerWindow = new ChangeCustomer();
            ChangeCustomerViewModel.ChangeCustomerDataEvent += dataBaseProcessing.UpdateCustomerRequest;
            ChangeCustomerEvent?.Invoke(row);
            changeCustomerWindow.ShowDialog();
            ChangeCustomerViewModel.ChangeCustomerDataEvent -= dataBaseProcessing.UpdateCustomerRequest;
        } 
        #endregion

        public ICommand RemoveClientCommand { get; }
        private bool CanRemoveClientCommandExecute (object parametr)
        {
            return true;
        }
        private void OnRemoveClientCommandExecuted(Object parameter)
        {
            dataBaseProcessing.RemoveCustomerRequest(parameter as DataRowView);
        }


        #endregion


    }
}
