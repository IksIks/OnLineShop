using Microsoft.EntityFrameworkCore;
using OnLineShop.Model;

namespace OnLineShop.DBContext;

public partial class ProductDbContext : DbContext
{
    public ProductDbContext()
    {
    }

    public ProductDbContext(DbContextOptions<ProductDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Shoppingcart> Shoppingcarts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;UserName=postgres;Password=1;Database=ProductDB");

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

        //OnModelCreatingPartial(modelBuilder);
    }

    //private partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}