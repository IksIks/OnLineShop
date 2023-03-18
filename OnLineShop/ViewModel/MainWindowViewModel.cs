using OnLineShop.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnLineShop.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {

        public string ClienBaseColorStatus { get; set; } = "Red";
        public string ProductBaseColorStatus { get; set; } = "Red";

        public MainWindowViewModel()
        {
            SqlConnectionStringBuilder connectionDBClients = new SqlConnectionStringBuilder()
            {
                
            };


        }




    }
}
