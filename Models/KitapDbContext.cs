using Microsoft.EntityFrameworkCore;
using books.Models.Entities;

#nullable disable

namespace books.Models;

public class KitapDbContext : DbContext
{
    public KitapDbContext(DbContextOptions<KitapDbContext> options) : base(options)
    {
    }

    public DbSet<Yazarlar> Yazarlars { get; set; }
    public DbSet<Kitaplar> Kitaplars { get; set; }
    public DbSet<Diller> Dillers { get; set; }
    public DbSet<Turler> Turlers { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Yayinevleri> Yayinevleris { get; set; }
    public DbSet<Turlertokitaplar> Turlertokitaplars { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Yazarlar>(entity =>
        {
            entity.ToTable("Yazarlar");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Adi).IsRequired();
            entity.Property(e => e.Soyadi).IsRequired();
            entity.Property(e => e.DogumTarihi);
            entity.Property(e => e.DogumYeri);
            entity.Property(e => e.Cinsiyeti);
        });

        modelBuilder.Entity<Kitaplar>(entity =>
        {
            entity.ToTable("Kitaplar");
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<Diller>(entity =>
        {
            entity.ToTable("Diller");
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<Turler>(entity =>
        {
            entity.ToTable("Turler");
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<Yayinevleri>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("yayinevleri");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.yayineviAdi)
                .HasMaxLength(100)
                .HasColumnName("yayineviadi");
            entity.Property(e => e.adres)
                .HasMaxLength(250)
                .HasColumnName("adres");
            entity.Property(e => e.tel)
                .HasMaxLength(20)
                .HasColumnName("tel");
            entity.Property(e => e.sira)
                .HasColumnName("sira");
        });

        modelBuilder.Entity<Turlertokitaplar>(entity =>
        {
            entity.ToTable("Turlertokitaplar");
            entity.HasKey(e => e.Id);
        });
    }
} 