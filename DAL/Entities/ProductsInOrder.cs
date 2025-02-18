using System;
using System.Collections.Generic;


namespace DAL.Abstractions.Entities;
public partial class ProductsInOrder
{
    public int OrderNumber { get; set; }

    public int ProductId { get; set; }

    public int Count { get; set; }

    public decimal PriceInOrder { get; set; }

    public virtual Order OrderNumberNavigation { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
