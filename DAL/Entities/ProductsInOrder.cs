using System;
using System.Collections.Generic;

namespace DAL.EFCore.PostgreSQL;

public partial class ProductsInOrder
{
    public long OrderNumber { get; set; }

    public long ProductId { get; set; }

    public int Count { get; set; }

    public decimal PriceInOrder { get; set; }

    public virtual Order OrderNumberNavigation { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
