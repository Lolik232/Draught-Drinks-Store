using System;
using System.Collections.Generic;


namespace DAL.Abstractions.Entities;

public partial class PaymentInExternalService
{
    public int Id { get; set; }

    public string ExternalPaymentId { get; set; } = null!;

    public string ExternalService { get; set; } = null!;

    public virtual Payment IdNavigation { get; set; } = null!;
}
