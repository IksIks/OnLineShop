using OnLineShop.Command;
using OnLineShop.Model;
using OnLineShop.Model.INPC;
using System;
using System.Data;
using System.Windows;
using System.Windows.Input;

namespace OnLineShop.ViewModel
{
    internal class ChangeCustomerViewModel : BaseINPC
    {
        public static event Action<Customer> ChangeCustomerDataEvent;
        private Customer newCustomer;
        public Customer NewCustomer
        {
            get => newCustomer;
            set => Set(ref newCustomer, value);
        }

        public ChangeCustomerViewModel()
        {
            newCustomer = new Customer();
            MainWindowViewModel.ChangeCustomerEvent += MainWindowViewModel_ChangeCustomer;
            SaveChanges = new LambdaCommand(OnSaveChangesExecuted, CanSaveChangesExecute);
            CancelButtonCommand = new LambdaCommand(OnCancelButtonCommandExecuted, CanCancelButtonCommandExecute);
        }

        private void MainWindowViewModel_ChangeCustomer(DataRow row)
        {
            NewCustomer.ID = (int)row["ID"];
            NewCustomer.Surname = row["Surname"].ToString();
            NewCustomer.Name = row["name"].ToString();
            NewCustomer.Patronymic = row["Patronymic"].ToString();
            NewCustomer.PhoneNumber = row["PhoneNumber"].ToString();
            NewCustomer.Email = row["Email"].ToString();
        }



        #region Команда сохранения изменений в данных пользователя
        public ICommand SaveChanges { get; }
        private bool CanSaveChangesExecute(object parameter)
        {
            if (String.IsNullOrEmpty(NewCustomer.Surname)
                    || String.IsNullOrEmpty(NewCustomer.Name)
                    || String.IsNullOrEmpty(NewCustomer.Patronymic)
                    || String.IsNullOrEmpty(NewCustomer.Email))
                return false;
            return true;
        }

        private void OnSaveChangesExecuted(object parameter)
        {
            ChangeCustomerDataEvent?.Invoke(NewCustomer);
            Application.Current.Windows[1].Close();
        } 
        #endregion


        #region Команда отмены добавления пользователя
        public ICommand CancelButtonCommand { get; }
        private bool CanCancelButtonCommandExecute(object parameter) => true;
        private void OnCancelButtonCommandExecuted(object parameter)
        {
            Application.Current.Windows[1].Close();
        }
        #endregion
    }
}
