using System;
using System.Collections.Generic;

namespace DAL.EFCore.PostgreSQL;

public partial class PaymentInExternalService
{
    public long Id { get; set; }

    public string ExternalPaymentId { get; set; } = null!;

    public string ExternalService { get; set; } = null!;

    public virtual Payment IdNavigation { get; set; } = null!;
}
