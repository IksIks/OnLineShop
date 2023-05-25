using OnLineShop.Model;
using OnLineShop.ViewModel.Base;
using System;
using System.Data;

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
            MainWindowViewModel.ChangeCustomer += MainWindowViewModel_ChangeCustomer;
        }

        private void MainWindowViewModel_ChangeCustomer(DataRow row)
        {
            NewCustomer.Surname = row["Surname"].ToString();
        }
    }
}
