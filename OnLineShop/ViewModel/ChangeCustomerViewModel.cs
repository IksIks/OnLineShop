using OnLineShop.Command;
using OnLineShop.Model;
using OnLineShop.Model.INPC;
using System;
using System.Windows;
using System.Windows.Input;

namespace OnLineShop.ViewModel
{
    internal class ChangeCustomerViewModel : BaseINPC
    {
        public static event Action<Client> ChangeCustomerDataEvent;

        private Client newClient;

        public Client NewClient
        {
            get => newClient;
            set => Set(ref newClient, value);
        }

        public ChangeCustomerViewModel()
        {
            newClient = new Client();
            MainWindowViewModel.ChangeCustomerEvent += MainWindowViewModel_ChangeCustomer;
            SaveChangesCommand = new LambdaCommand(OnSaveChangesExecuted, CanSaveChangesExecute);
            CancelButtonCommand = new LambdaCommand(OnCancelButtonCommandExecuted, CanCancelButtonCommandExecute);
        }

        private void MainWindowViewModel_ChangeCustomer(Client client)
        {
            NewClient.Id = client.Id;
            NewClient.Surname = client.Surname.ToString();
            NewClient.Name = client.Name.ToString();
            NewClient.Patronymic = client.Patronymic.ToString();
            NewClient.PhoneNumber = client.PhoneNumber.ToString();
            NewClient.Email = client.Email.ToString();
        }

        #region Команда сохранения изменений в данных пользователя

        public ICommand SaveChangesCommand { get; }

        private bool CanSaveChangesExecute(object parameter)
        {
            if (String.IsNullOrEmpty(NewClient.Surname)
                    || String.IsNullOrEmpty(NewClient.Name)
                    || String.IsNullOrEmpty(NewClient.Patronymic)
                    || String.IsNullOrEmpty(NewClient.Email))
                return false;
            return true;
        }

        private void OnSaveChangesExecuted(object parameter)
        {
            ChangeCustomerDataEvent?.Invoke(NewClient);
            Application.Current.Windows[1].Close();
        }

        #endregion Команда сохранения изменений в данных пользователя

        #region Команда отмены добавления пользователя

        public ICommand CancelButtonCommand { get; }

        private bool CanCancelButtonCommandExecute(object parameter) => true;

        private void OnCancelButtonCommandExecuted(object parameter)
        {
            Application.Current.Windows[1].Close();
        }

        #endregion Команда отмены добавления пользователя
    }
}