using OnLineShop.ViewModel.Base;
using System.Data;

namespace OnLineShop.ViewModel
{
    internal class CustomerProductViewModel : ViewModelBase
    {
        private DataTable productCustomerDBGrid;

        public DataTable ProductCustomerDBGrid
        {
            get => productCustomerDBGrid;
            set => Set(ref productCustomerDBGrid, value);
        }

        public CustomerProductViewModel()
        {
            MainWindowViewModel.ViewProductCustomerTableEvent += MainWindowViewModel_ViewProductCustomerTableEvent;
        }

        private void MainWindowViewModel_ViewProductCustomerTableEvent(DataTable obj)
        {
            productCustomerDBGrid = new DataTable();
            ProductCustomerDBGrid = obj;
        }
    }
}
