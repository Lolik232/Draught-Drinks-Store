using System;
using System.Collections.Generic;

namespace DAL.Abstractions.Entities;

public partial class Payment
{
    public int Id { get; set; }

    public string Status { get; set; } = null!;

    public decimal Sum { get; set; }

    public virtual Order? Order { get; set; }
    public virtual PaymentInExternalService? PaymentInExternalService { get; set; }
}
