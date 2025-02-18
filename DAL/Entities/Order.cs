using System;
using System.Collections.Generic;


namespace DAL.Abstractions.Entities;
public partial class Order
{
    public int Number { get; set; }

    public DateTime Date { get; set; }

    public string Status { get; set; } = null!;

    public int PaymentId { get; set; }

    public int ContactDataId { get; set; }

    public string? PromoCode { get; set; }

    public virtual ContactData? ContactData { get; set; }

    public virtual Payment? Payment { get; set; }

    public virtual ICollection<ProductsInOrder> ProductsInOrders { get; set; } = new List<ProductsInOrder>();
}
