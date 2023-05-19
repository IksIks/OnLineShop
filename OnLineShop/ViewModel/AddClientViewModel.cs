using OnLineShop.Command;
using OnLineShop.Model;
using System;
using System.Windows.Input;

namespace OnLineShop.ViewModel
{
    internal class AddClientViewModel
    {
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

        }
    }
}
