using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OnLineShop;

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
                .HasColumnName("id");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasColumnName("email");
            entity.Property(e => e.Productcode).HasColumnName("productcode");
            entity.Property(e => e.Productname)
                .IsRequired()
                .HasColumnName("productname");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
