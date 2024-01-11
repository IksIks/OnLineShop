﻿using OnLineShop.Command;
using OnLineShop.Data;
using OnLineShop.Model;
using OnLineShop.View;
using OnLineShop.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Data;
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

        private IEnumerable<Client> clientsDataGridItemTable;
        private IEnumerable<Shoppingcart> productDataGridItemTable;

        public static event Action<DataRow> ChangeCustomerEvent;

        public static event Action<DataTable> ViewProductCustomerTableEvent;

        public IEnumerable<Shoppingcart> ProductDataGridItemTable
        {
            get => productDataGridItemTable;
            set => Set(ref productDataGridItemTable, value);
        }

        public IEnumerable<Client> ClientsDataGridItemTable
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
            UpdateCustomerDataCommand = new LambdaCommand(OnUpdateCustomerDataCommandExecuted, CanUpdateCustomerDataCommandExecute);
            RemoveClientCommand = new LambdaCommand(OnRemoveClientCommandExecuted, CanRemoveClientCommandExecute);
            CustomerProductCommand = new LambdaCommand(OnCustomerProductCommandExecuted, CanCustomerProductCommandExecute);
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
                    ClientsDataGridItemTable = await dataBaseProcessing.GetDataFromDBAsync(dbChoise) as IEnumerable<Client>;
                }
                else
                {
                    ProductDBColorStatus = "Green";
                    ProductDataGridItemTable = await dataBaseProcessing.GetDataFromDBAsync(dbChoise) as IEnumerable<Shoppingcart>;
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

        private void OnAddClientCommandExecuted(object parameter)
        {
            AddClientViewModel.AddNewClient += dataBaseProcessing.InsertNewCustomerRequest;
            AddClient addClient = new AddClient();
            addClient.ShowDialog();
            AddClientViewModel.AddNewClient -= dataBaseProcessing.InsertNewCustomerRequest;
        }

        #endregion Команда добавления клиента

        //---------------------------------------------------------------------------------------------------------------------------------

        #region Команда обновления данных о клиенте

        public ICommand UpdateCustomerDataCommand { get; }

        private bool CanUpdateCustomerDataCommandExecute(object parameter)
        {
            return (parameter is DataRowView);
        }

        private void OnUpdateCustomerDataCommandExecuted(object parameter)
        {
            var row = (parameter as DataRowView).Row;
            ChangeCustomer changeCustomerWindow = new ChangeCustomer();
            //ChangeCustomerViewModel.ChangeCustomerDataEvent += dataBaseProcessing.UpdateCustomerRequest;
            ChangeCustomerEvent?.Invoke(row);
            changeCustomerWindow.ShowDialog();
            //ChangeCustomerViewModel.ChangeCustomerDataEvent -= dataBaseProcessing.UpdateCustomerRequest;
        }

        #endregion Команда обновления данных о клиенте

        //---------------------------------------------------------------------------------------------------------------------------------

        #region Команда удаления клиента

        public ICommand RemoveClientCommand { get; }

        private bool CanRemoveClientCommandExecute(object parameter)
        {
            return (parameter is DataRowView);
        }

        private void OnRemoveClientCommandExecuted(Object parameter)
        {
            if (MessageBox.Show("Вы уверены", "Подтверждение удаления клиента", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes) == MessageBoxResult.Yes) ;
            //dataBaseProcessing.RemoveCustomerRequest(parameter as DataRowView);
            else MessageBox.Show("Слабак :-))))");
        }

        #endregion Команда удаления клиента

        //---------------------------------------------------------------------------------------------------------------------------------

        #region Команда просмотра покупок

        public ICommand CustomerProductCommand { get; }

        private bool CanCustomerProductCommandExecute(object parameter)
        {
            return (parameter is DataRowView) && ProductDataGridItemTable != null;
        }

        private async void OnCustomerProductCommandExecuted(object parameter)
        {
            string email = (parameter as DataRowView).Row[5].ToString();
            CustomerProductView producWindow = new CustomerProductView();

            //ViewProductCustomerTableEvent?.Invoke(await dataBaseProcessing.CustomerProductRequest(email));
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