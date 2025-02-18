using System;
using System.Collections.Generic;

namespace DAL.Abstractions.Entities;

public partial class ContactData
{
    public int Id { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual User? User { get; set; }
}
