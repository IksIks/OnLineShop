using OnLineShop.Command;
using OnLineShop.Model;
using OnLineShop.ViewModel.Base;
using System;
using System.Windows;
using System.Windows.Input;

namespace OnLineShop.ViewModel
{
    internal class AddClientViewModel : ViewModelBase
    {
        public static event Action<Client> AddNewClient;

        private Client newClient;

        public Client NewClient
        {
            get => newClient;
            set => Set(ref newClient, value);
        }

        public AddClientViewModel()
        {
            NewClient = new Client();
            AddButtonCommand = new LambdaCommand(OnAddButtonCommandExecited, CanAddButtonCommandExecute);
            CancelButtonCommand = new LambdaCommand(OnCancelButtonCommandExecuted, CanCancelButtonCommandExecute);
        }

        #region Команда добавление пользователя

        public ICommand AddButtonCommand { get; }

        private bool CanAddButtonCommandExecute(object parameter)
        {
            if (String.IsNullOrEmpty(NewClient.Surname)
                    || String.IsNullOrEmpty(NewClient.Name)
                    || String.IsNullOrEmpty(NewClient.Patronymic)
                    || String.IsNullOrEmpty(NewClient.Email))
                return false;
            return true;
        }

        private void OnAddButtonCommandExecited(object parameter)
        {
            AddNewClient?.Invoke(NewClient);
            Application.Current.Windows[1].Close();
        }

        #endregion Команда добавление пользователя

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