using Microsoft.EntityFrameworkCore;
using OnLineShop.Model;
using OnLineShop.DBContext.ConnectionSettings;

namespace OnLineShop.DBContext;

public partial class ProductDbContext : DbContext
{
    public ProductDbContext()
    {
        Database.EnsureCreated();
    }

    public ProductDbContext(DbContextOptions<ProductDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Shoppingcart> Shoppingcarts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
                => optionsBuilder.UseNpgsql(ConSettings.GetConnectionString("PSGSQL"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Shoppingcart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ShoppingCart_pkey");

            entity.ToTable("shoppingcart", tb => tb.HasComment("покупки клиентов\n•	ID\n•	Email\n•	Код товара\n•	Наименование товара\n"));

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("ID");
            entity.Property(e => e.Email).IsRequired();
            entity.Property(e => e.ProductName).IsRequired();
        });
    }
}