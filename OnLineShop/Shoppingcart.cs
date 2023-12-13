using System;
using System.Collections.Generic;

namespace OnLineShop;

/// <summary>
/// покупки клиентов
/// •	ID
/// •	Email
/// •	Код товара
/// •	Наименование товара
/// 
/// </summary>
public partial class Shoppingcart
{
    public int Id { get; set; }

    public string Email { get; set; }

    public int Productcode { get; set; }

    public string Productname { get; set; }
}
