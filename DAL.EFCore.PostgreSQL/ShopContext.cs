using Microsoft.EntityFrameworkCore;
using DAL.Abstractions.Entities;

namespace DAL.EFCore.PostgreSQL;

public partial class ShopContext : DbContext
{
    public ShopContext(DbContextOptions<ShopContext> options)
        : base(options)
    {
    }

    public ShopContext()
    {
    }


    public virtual DbSet<ContactData> ContactData { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<PaymentInExternalService> PaymentInExternalServices { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductsInOrder> ProductsInOrders { get; set; }

    public virtual DbSet<ProductsInStock> ProductsInStocks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ContactData>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("contact_data_pkey");

            entity.ToTable("contact_data");

            entity.HasIndex(e => e.PhoneNumber, "contact_data_phone_number_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .HasColumnName("phone_number");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Number).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.Property(e => e.Number).HasColumnName("number");
            entity.Property(e => e.ContactDataId).HasColumnName("contact_data_id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.PaymentId).HasColumnName("payment_id");
            entity.Property(e => e.PromoCode)
                .HasMaxLength(50)
                .HasColumnName("promo_code");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .HasColumnName("status");

            entity.HasOne(d => d.ContactData).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ContactDataId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orders_contact_data_id_fkey");

            entity.HasOne(d => d.Payment).WithOne(p => p.Order)
                .HasForeignKey<Order>(d => d.PaymentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orders_payment_id_fkey");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("payments_pkey");

            entity.ToTable("payments");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .HasColumnName("status");
            entity.Property(e => e.Sum)
                .HasColumnType("money")
                .HasColumnName("sum");
        });

        modelBuilder.Entity<PaymentInExternalService>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("payment_in_external_services_pkey");

            entity.ToTable("payment_in_external_services");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.ExternalPaymentId)
                .HasMaxLength(50)
                .HasColumnName("external_payment_id");
            entity.Property(e => e.ExternalService)
                .HasMaxLength(50)
                .HasColumnName("external_service");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.PaymentInExternalService)
                .HasForeignKey<PaymentInExternalService>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("payment_in_external_services_id_fkey");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("products_pkey");

            entity.ToTable("products");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(100)
                .HasColumnName("image_url");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("money")
                .HasColumnName("price");
        });

        modelBuilder.Entity<ProductsInOrder>(entity =>
        {
            entity.HasKey(e => new { e.OrderNumber, e.ProductId }).HasName("products_in_orders_pkey");

            entity.ToTable("products_in_orders");

            entity.Property(e => e.OrderNumber).HasColumnName("order_number");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.PriceInOrder)
                .HasColumnType("money")
                .HasColumnName("price_in_order");

            entity.HasOne(d => d.OrderNumberNavigation).WithMany(p => p.ProductsInOrders)
                .HasForeignKey(d => d.OrderNumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("products_in_orders_order_number_fkey");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductsInOrders)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("products_in_orders_product_id_fkey");
        });

        modelBuilder.Entity<ProductsInStock>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("products_in_stock_pkey");

            entity.ToTable("products_in_stock");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.ProductId).HasColumnName("product_id");

            entity.HasOne(d => d.Product).WithOne(p => p.ProductsInStocks)
                .HasForeignKey<ProductsInStock>(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("products_in_stock_product_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ContactDataId).HasColumnName("contact_data_id");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.EmailConfirmed).HasColumnName("email_confirmed");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(50)
                .HasColumnName("password_hash");
            entity.Property(e => e.Role)
                .HasMaxLength(10)
                .HasColumnName("role");

            entity.HasOne(d => d.ContactData).WithOne(p => p.User)
                .HasForeignKey<User>(d => d.ContactDataId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_contact_data_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}