using OnLineShop.Model;
using OnLineShop.ViewModel.Base;
using System.Collections.Generic;

namespace OnLineShop.ViewModel
{
    internal class CustomerProductViewModel : ViewModelBase
    {
        private IEnumerable<Shoppingcart> productCustomerDBGrid;

        public IEnumerable<Shoppingcart> ProductCustomerDBGrid
        {
            get => productCustomerDBGrid;
            set => Set(ref productCustomerDBGrid, value);
        }

        public CustomerProductViewModel()
        {
            MainWindowViewModel.ViewProductCustomerTableEvent += MainWindowViewModel_ViewProductCustomerTableEvent;
        }

        private void MainWindowViewModel_ViewProductCustomerTableEvent(IEnumerable<Shoppingcart> obj)
        {
            ProductCustomerDBGrid = obj;
        }
    }
}