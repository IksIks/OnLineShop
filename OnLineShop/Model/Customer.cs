using OnLineShop.ViewModel.Base;
using System.Data;

namespace OnLineShop.Model
{
    internal class Customer : ViewModelBase
    {
        public int ID { get; set; }
        private string surname;
        public string Surname
        { get => surname;
          set => Set(ref surname, value);
        }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public long PhoneNumber { get; set; }
        public string Email { get; set; }

       
    }
}
