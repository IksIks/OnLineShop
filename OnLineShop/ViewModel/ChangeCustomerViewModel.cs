using OnLineShop.Command;
using OnLineShop.Model;
using OnLineShop.ViewModel.Base;
using System;
using System.Data;
using System.Windows;
using System.Windows.Input;

namespace OnLineShop.ViewModel
{
    internal class ChangeCustomerViewModel : ViewModelBase
    {
        
        private Customer newCustomer;
        public Customer NewCustomer
        {
            get => newCustomer;
            set => Set(ref newCustomer, value);
        }

        public ChangeCustomerViewModel()
        {
            newCustomer = new Customer();
            MainWindowViewModel.ChangeCustomerIn += MainWindowViewModel_ChangeCustomer;
            TestCommand = new LambdaCommand(OnTestCommandExecuted, CanTestCommandExecute);
        }

        private void MainWindowViewModel_ChangeCustomer(DataRow row)
        {
            NewCustomer.Surname = row["Surname"].ToString();
        }



        public ICommand TestCommand { get; }
        private bool CanTestCommandExecute(object parameter) => true;
        private void OnTestCommandExecuted(object parameter)
        {
            MessageBox.Show(NewCustomer.Surname);
        }
    }
}
