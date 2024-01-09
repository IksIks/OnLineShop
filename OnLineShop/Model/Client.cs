using OnLineShop.Model.INPC;

namespace OnLineShop.Model;

public class Client : BaseINPC
{
    private int id;
    private string surname;
    private string name;
    private string patronymic;
    private string phoneNumber;
    private string email;

    public int Id
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

    public string PhoneNumber
    {
        get => phoneNumber;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                Set(ref phoneNumber, null);
            Set(ref phoneNumber, value);
        }
    }

    public string Email
    {
        get => email;
        set => Set(ref email, value);
    }
}