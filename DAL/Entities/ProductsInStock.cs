using System;
using System.Collections.Generic;

namespace DAL.EFCore.PostgreSQL;

public partial class ProductsInStock
{
    public long Id { get; set; }

    public long ProductId { get; set; }

    public int Count { get; set; }

    public virtual Product Product { get; set; } = null!;
}
