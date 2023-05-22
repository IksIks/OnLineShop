using OnLineShop.Command;
using OnLineShop.Model;
using OnLineShop.ViewModel.Base;
using System;
using System.Windows;
using System.Windows.Input;

namespace OnLineShop.ViewModel
{
    internal class AddClientViewModel:ViewModelBase
    {

        public static event Action<Customer> AddNewCustomer;
        private Customer newCustomer;
        public Customer NewCustomer
        {
            get => newCustomer;
            set => Set(ref newCustomer, value);
        }

        public AddClientViewModel()
        {
            NewCustomer = new Customer();
            AddButtonCommand = new LambdaCommand(OnAddButtonCommandExecited, CanAddButtonCommandExecute);
        }

        public ICommand AddButtonCommand { get; }
        private bool CanAddButtonCommandExecute(object parameter)
        {
            if ( String.IsNullOrEmpty(NewCustomer.Surname)
                || String.IsNullOrEmpty(NewCustomer.Name)
                || String.IsNullOrEmpty(NewCustomer.Patronymic)
                || String.IsNullOrEmpty(NewCustomer.Email))
                return false;
            return true;
        }
        private void OnAddButtonCommandExecited(object parameter)
        {
            AddNewCustomer?.Invoke(NewCustomer);
            Application.Current.Windows[1].Close();
        }


    }
}
