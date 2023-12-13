using System;
using System.Collections.Generic;

namespace DAL.EFCore.PostgreSQL;

public partial class Payment
{
    public long Id { get; set; }

    public DateTime Status { get; set; }

    public decimal Sum { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual PaymentInExternalService? PaymentInExternalService { get; set; }
}
