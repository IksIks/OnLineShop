using OnLineShop.Command;
using OnLineShop.Data;
using OnLineShop.Model;
using OnLineShop.View;
using OnLineShop.ViewModel.Base;
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

        private DataTable clientsDataGridItemTable = new DataTable();
        private DataTable productDataGridItemTable;
        
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
            AddClientViewModel.AddNewCustomer += AddClientViewModel_TestEvent;
            AddClient addClient = new AddClient();
            addClient.ShowDialog();
        }

        private void AddClientViewModel_TestEvent(Customer arg)
        {
            Customer test = arg as Customer;
            DataRow row = ClientsDataGridItemTable.NewRow();
            row["Surname"] = test.Surname;
        }

        #endregion

        #endregion

        public void AddRow(Customer customer)
        {
            //if (customer == null)
            //DataRow row = ClientsDataGridItemTable.NewRow();
            //row["Surname"] = customer.Surname;
            
        }


    }
}
