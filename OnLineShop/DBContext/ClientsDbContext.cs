using Microsoft.EntityFrameworkCore;
using OnLineShop.DBContext.ConnectionSettings;
using OnLineShop.Model;

namespace OnLineShop.DBContext;

public partial class ClientsDbContext : DbContext
{
    public ClientsDbContext()
    {
        Database.EnsureCreated();
    }

    public ClientsDbContext(DbContextOptions<ClientsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
                => optionsBuilder.UseSqlServer(ConSettings.GetConnectionString("SQL"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Email).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Patronymic).IsRequired().HasMaxLength(50);
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            entity.Property(e => e.Surname).IsRequired().HasMaxLength(50);
        });
    }
}