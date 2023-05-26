using OnLineShop.Model.INPC;

namespace OnLineShop.Model
{
    internal class Customer : BaseINPC
    {
        private int id;
        private string surname;
        private string name;
        private string patronymic;
        private long phoneNumber;
        private string email;



        public int ID
        {
            get => id;
            set => Set(ref id, value);
        }
        public string Surname
        { 
            get => surname;
          set => Set(ref surname, value);
        }
        public string Name
        {
            get => name;
            set => Set(ref name, value);
        }
        public string Patronymic
        {
            get => patronymic;
            set => Set(ref patronymic, value);
        }
        public long PhoneNumber
        {
            get => phoneNumber;
            set => Set(ref phoneNumber, value);
        }
        public string Email
        {
            get => email;
            set => Set(ref email, value);
        }

    }
}
