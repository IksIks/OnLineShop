using System.Data;

namespace OnLineShop.Model
{
    internal class Customer
    {
        public int ID { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public long PhoneNumber { get; set; }
        public string Email { get; set; }

       
    }
}
