﻿namespace OnLineShop.Model;

public partial class Shoppingcart
{
    public int Id { get; set; }

    public string Email { get; set; }

    public int ProductCode { get; set; }

    public string ProductName { get; set; }
}