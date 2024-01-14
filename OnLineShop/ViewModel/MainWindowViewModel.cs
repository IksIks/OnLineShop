﻿using OnLineShop.Command;
using OnLineShop.Data;
using OnLineShop.Model;
using OnLineShop.View;
using OnLineShop.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace OnLineShop.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private string testConntection;
        private string clienDBColorStatus = "Red", productDBColorStatus = "Red";
        private string dbChoise;
        private DatabaseProcessing dataBaseProcessing;

        private ObservableCollection<Client> clientsDataGridItemTable;
        private List<Shoppingcart> productDataGridItemTable;

        public static event Action<Client> ChangeCustomerEvent;

        public static event Action<List<Shoppingcart>> ViewProductCustomerTableEvent;

        public List<Shoppingcart> ProductDataGridItemTable
        {
            get => productDataGridItemTable;
            set => Set(ref productDataGridItemTable, value);
        }

        public ObservableCollection<Client> ClientsDataGridItemTable
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
            dataBaseProcessing = new DatabaseProcessing();
            ConnectClientDBCommand = new LambdaCommand(OnConnectClientDBCommandExecuted, CanConnectClientDBCommandExecute);
            AddClientCommand = new LambdaCommand(OnAddClientCommandExecuted, CanAddClientCommandExecute);
            UpdateClientDataCommand = new LambdaCommand(OnUpdateClientDataCommandExecuted, CanUpdateClientDataCommandExecute);
            RemoveClientCommand = new LambdaCommand(OnRemoveClientCommandExecuted, CanRemoveClientCommandExecute);
            ClientProductCommand = new LambdaCommand(OnClientProductCommandExecuted, CanClientProductCommandExecute);
            AboutProgrammCommand = new LambdaCommand(OnAboutProgrammCommandExecuted, CanAboutProgrammCommandExecute);
        }

        #region Команды

        #region Команда загрузки базы

        public ICommand ConnectClientDBCommand { get; }

        private bool CanConnectClientDBCommandExecute(object parameter)
        {
            return (clientsDataGridItemTable is null || productDataGridItemTable is null);
        }

        private async void OnConnectClientDBCommandExecuted(object parameter)
        {
            dbChoise = parameter as string;
            bool answer = await dataBaseProcessing.StartConnectionDBAsync(dbChoise);
            if (Equals(answer, true))
            {
                if (dbChoise == "ClientsDB")
                {
                    ClienDBColorStatus = "Green";
                    ClientsDataGridItemTable = new ObservableCollection<Client>(await dataBaseProcessing.GetDataFromDBAsync(dbChoise) as List<Client>);
                }
                else
                {
                    ProductDBColorStatus = "Green";
                    ProductDataGridItemTable = await dataBaseProcessing.GetDataFromDBAsync(dbChoise) as List<Shoppingcart>;
                }
            }
        }

        #endregion Команда загрузки базы

        //---------------------------------------------------------------------------------------------------------------------------------

        #region Команда добавления клиента

        public ICommand AddClientCommand { get; }

        private bool CanAddClientCommandExecute(object parametr)
        {
            return (clientsDataGridItemTable != null);
        }

        private async void OnAddClientCommandExecuted(object parameter)
        {
            AddClientViewModel.AddNewClient += dataBaseProcessing.InsertNewCustomerRequest;
            AddClient addClient = new AddClient();
            addClient.ShowDialog();
            AddClientViewModel.AddNewClient -= dataBaseProcessing.InsertNewCustomerRequest;
            ClientsDataGridItemTable = new ObservableCollection<Client>(await dataBaseProcessing.GetDataFromDBAsync("ClientsDB") as List<Client>);
        }

        #endregion Команда добавления клиента

        //---------------------------------------------------------------------------------------------------------------------------------

        #region Команда обновления данных о клиенте

        public ICommand UpdateClientDataCommand { get; }

        private bool CanUpdateClientDataCommandExecute(object parameter)
        {
            return (parameter is Client);
        }

        private async void OnUpdateClientDataCommandExecuted(object parameter)
        {
            var client = parameter as Client;
            ChangeCustomer changeCustomerWindow = new ChangeCustomer();
            ChangeCustomerViewModel.ChangeCustomerDataEvent += dataBaseProcessing.UpdateCustomerRequest;
            ChangeCustomerEvent?.Invoke(client);
            changeCustomerWindow.ShowDialog();
            ChangeCustomerViewModel.ChangeCustomerDataEvent -= dataBaseProcessing.UpdateCustomerRequest;
            ClientsDataGridItemTable = new ObservableCollection<Client>(await dataBaseProcessing.GetDataFromDBAsync("ClientsDB") as List<Client>);
        }

        #endregion Команда обновления данных о клиенте

        //---------------------------------------------------------------------------------------------------------------------------------

        #region Команда удаления клиента

        public ICommand RemoveClientCommand { get; }

        private bool CanRemoveClientCommandExecute(object parameter)
        {
            return (parameter is Client);
        }

        private void OnRemoveClientCommandExecuted(Object parameter)
        {
            var tempClient = parameter as Client;
            if (MessageBox.Show("Вы уверены", "Подтверждение удаления клиента", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes)
            {
                dataBaseProcessing.RemoveCustomerRequest(tempClient);
                ClientsDataGridItemTable.Remove(tempClient);
            }
            else MessageBox.Show("Слабак :-))))");
        }

        #endregion Команда удаления клиента

        //---------------------------------------------------------------------------------------------------------------------------------

        #region Команда просмотра покупок

        public ICommand ClientProductCommand { get; }

        private bool CanClientProductCommandExecute(object parameter)
        {
            return (parameter is Client) && ProductDataGridItemTable != null;
        }

        private void OnClientProductCommandExecuted(object parameter)
        {
            string email = (parameter as Client).Email;
            CustomerProductView producWindow = new CustomerProductView();

            ViewProductCustomerTableEvent?.Invoke(dataBaseProcessing.ClientProductRequest(email));
            producWindow.ShowDialog();
        }

        #endregion Команда просмотра покупок

        //---------------------------------------------------------------------------------------------------------------------------------

        #region Команда "О программе"

        public ICommand AboutProgrammCommand { get; }

        private bool CanAboutProgrammCommandExecute(object parameter) => true;

        private void OnAboutProgrammCommandExecuted(object parameter)
        {
            MessageBox.Show("Created by IKS. Отдельно спасибо за терпение моей жене, сыну и коту. " +
                "А так же большой респект видео хостингу YouTube и отдельным сайтам где я искал информацию. " +
                "Если Вам понравилась программа - ставьте лайки (кнопку я не делал, да и не буду), " +
                "если не понравилась - можете жаловаться в ООН");
        }

        #endregion Команда "О программе"

        #endregion Команды
    }
}