using OnLineShop.Command;
using OnLineShop.Data;
using OnLineShop.Model;
using System;
using System.Data;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace OnLineShop.ViewModel
{
    internal class AddClientViewModel
    {

        public static event Action<Customer> AddNewCustomer;
        public Customer NewCustomer { get; set; }

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
            Application.Current.Windows[1].Close();
            AddNewCustomer(NewCustomer);

        }

        
    }
}
