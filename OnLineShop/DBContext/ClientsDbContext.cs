using Microsoft.EntityFrameworkCore;
using OnLineShop.Model;

namespace OnLineShop.DBContext;

public partial class ClientsDbContext : DbContext
{
    public ClientsDbContext()
    {
    }

    public ClientsDbContext(DbContextOptions<ClientsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=(localDB)\\MSSQLLocalDB;AttachDbFilename=C:\\YandexDisk\\IKS\\C#_проекты\\Проекты\\OnLineShop\\OnLineShop\\DB\\ClientsDB.mdf;Database=ClientsDB;TrustServerCertificate=true;Trusted_Connection=True");

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

        //OnModelCreatingPartial(modelBuilder);
    }

    //private partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}