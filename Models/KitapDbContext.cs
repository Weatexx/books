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
    public DbSet<Kullanicilar> Kullanicilars { get; set; }
    public DbSet<Iletisim> Iletisims { get; set; } = null!;

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

        modelBuilder.Entity<Kullanicilar>(entity =>
        {
            entity.HasKey(e => e.id);
            entity.ToTable("kullanicilar");
            entity.Property(e => e.usernames).HasMaxLength(50).IsRequired();
            entity.Property(e => e.passwords).HasMaxLength(50).IsRequired();
            entity.Property(e => e.isim).HasMaxLength(50).HasDefaultValue("");
            entity.Property(e => e.soyisim).HasMaxLength(50).HasDefaultValue("");
            entity.Property(e => e.telno).HasDefaultValue(0);
            entity.Property(e => e.resim).HasMaxLength(50).HasDefaultValue("default.jpg");
        });

        modelBuilder.Entity<Iletisim>(entity =>
        {
            entity.ToTable("iletisim");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Id)
                .HasColumnName("id");
            
            entity.Property(e => e.AdSoyad)
                .HasColumnName("adsoyad")
                .HasColumnType("varchar(50)")
                .IsRequired();
            
            entity.Property(e => e.Email)
                .HasColumnName("eposta")
                .HasColumnType("varchar(100)")
                .IsRequired();
            
            entity.Property(e => e.Konu)
                .HasColumnName("konu")
                .HasColumnType("varchar(150)")
                .IsRequired();
            
            entity.Property(e => e.Mesaj)
                .HasColumnName("mesaj")
                .HasColumnType("varchar(500)")
                .IsRequired();
            
            entity.Property(e => e.TarihSaat)
                .HasColumnName("tarihSaat")
                .HasColumnType("datetime");
            
            entity.Property(e => e.Ip)
                .HasColumnName("ip")
                .HasColumnType("char(50)");
            
            entity.Property(e => e.Goruldu)
                .HasColumnName("goruldu")
                .HasColumnType("tinyint(1)");
        });
    }
} 