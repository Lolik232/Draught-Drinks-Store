using System;
using System.Collections.Generic;

namespace DAL.EFCore.PostgreSQL;

public partial class Product
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;

    public decimal Price { get; set; }

    public long CategoryId { get; set; }

    public virtual ICollection<ProductsInOrder> ProductsInOrders { get; set; } = new List<ProductsInOrder>();

    public virtual ICollection<ProductsInStock> ProductsInStocks { get; set; } = new List<ProductsInStock>();
}
