using System;
using System.Collections.Generic;


namespace DAL.Abstractions.Entities;

public partial class ProductsInStock
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int Count { get; set; }

    public virtual Product Product { get; set; } = null!;
}
