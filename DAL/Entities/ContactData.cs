using System;
using System.Collections.Generic;

namespace DAL.EFCore.PostgreSQL;

public partial class ContactData
{
    public long Id { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
