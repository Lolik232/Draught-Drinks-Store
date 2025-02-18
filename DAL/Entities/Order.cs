namespace DAL.EFCore.PostgreSQL;

public partial class Order
{
    public long Number { get; set; }

    public DateTime Date { get; set; }

    public string Status { get; set; } = null!;

    public long PaymentId { get; set; }

    public long ContactDataId { get; set; }

    public string? PromoCode { get; set; }

    public virtual ContactData ContactData { get; set; } = null!;

    public virtual Payment Payment { get; set; } = null!;

    public virtual ICollection<ProductsInOrder> ProductsInOrders { get; set; } = new List<ProductsInOrder>();
}
