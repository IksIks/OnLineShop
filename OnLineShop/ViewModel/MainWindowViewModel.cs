﻿using OnLineShop.Command;
using OnLineShop.Data;
using OnLineShop.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

        private DataTable clientsDataGridItem;
        private DataTable productDataGridItem;

        public DataTable ProductDataGridItem
        {
            get => productDataGridItem;
            set => Set(ref productDataGridItem, value);
        }

        public DataTable ClientsDataGridItem      
        {
            get => clientsDataGridItem;
            set => Set(ref clientsDataGridItem, value);
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
            dataBaseProcessing= new DatabaseProcessing();
            ConnectClientDBCommand = new LambdaCommand(OnConnectClientDBCommandExecuted, CanConnectClientDBCommandExecute);
        }

        #region Команды

        #region Команда загрузки базы

            public ICommand ConnectClientDBCommand { get; }
            private bool CanConnectClientDBCommandExecute(object parameter) => true;
            private async void OnConnectClientDBCommandExecuted(object parameter)
            {
                dbChoise= parameter as string;
                string answer = await dataBaseProcessing.StartConnectionDBAsync(dbChoise);
                if (Equals(answer, "Open"))
                {
                    if (dbChoise == "0")
                    {
                        ClienDBColorStatus = "Green";
                        ClientsDataGridItem = await dataBaseProcessing.FillClientsDataTable(dbChoise);
                    }
                    else
                    {
                        ProductDBColorStatus = "Green";
                        ProductDataGridItem = await dataBaseProcessing.FillProductDataTable(dbChoise);
                    }
                }
            }

        #endregion

        #endregion
        
            
     
    }
}
