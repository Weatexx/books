using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace books.Models.Entities;

public partial class KitapDbContext : DbContext
{
    public KitapDbContext()
    {
    }

    public KitapDbContext(DbContextOptions<KitapDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Diller> Dillers { get; set; }

    public virtual DbSet<Iletisim> Iletisims { get; set; }

    public virtual DbSet<Kitaplar> Kitaplars { get; set; }

    public virtual DbSet<Turler> Turlers { get; set; }

    public virtual DbSet<Turlertokitaplar> Turlertokitaplars { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Yayinevleri> Yayinevleris { get; set; }

    public virtual DbSet<Yazarlar> Yazarlars { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("latin5_turkish_ci")
            .HasCharSet("latin5");

        modelBuilder.Entity<Diller>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DilAdi).HasColumnName("dilAdi").HasColumnType("char(50)");
        });

        modelBuilder.Entity<Iletisim>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("iletisim");
            
            entity.Property(e => e.Id)
                .HasColumnName("id");
            
            entity.Property(e => e.AdSoyad)
                .HasColumnName("adsoyad")
                .HasColumnType("varchar(100)")
                .IsRequired();
            
            entity.Property(e => e.Email)
                .HasColumnName("email")
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

        modelBuilder.Entity<Kitaplar>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Adi).HasColumnName("adi").HasColumnType("char(200)");
            entity.Property(e => e.YazarId).HasColumnName("yazarID");
            entity.Property(e => e.DilId).HasColumnName("DilID");
            entity.Property(e => e.SayfaSayisi).HasColumnName("sayfaSayisi");
            entity.Property(e => e.YayineviId).HasColumnName("yayineviID");
            entity.Property(e => e.Ozet).HasColumnName("ozet").HasMaxLength(5000);
            entity.Property(e => e.YayinTarihi).HasColumnName("yayinTarihi");
            entity.Property(e => e.Resim).HasColumnName("resim").HasMaxLength(50);
        });

        modelBuilder.Entity<Turler>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.TurAdi).HasColumnName("turAdi").HasColumnType("char(50)");
            entity.Property(e => e.Sira).HasColumnName("Sira");
        });

        modelBuilder.Entity<Turlertokitaplar>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.KitapId).HasColumnName("kitapID");
            entity.Property(e => e.TurId).HasColumnName("turID");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Username).HasColumnName("username").HasMaxLength(50);
            entity.Property(e => e.Password).HasColumnName("password").HasMaxLength(50);
        });

        modelBuilder.Entity<Yayinevleri>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.yayineviAdi).HasColumnName("yayineviAdi").HasColumnType("char(200)");
            entity.Property(e => e.adres).HasColumnName("adres").HasMaxLength(150);
            entity.Property(e => e.tel).HasColumnName("tel").HasColumnType("char(11)");
            entity.Property(e => e.sira).HasColumnName("sira");
        });

        modelBuilder.Entity<Yazarlar>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Adi).HasColumnName("adi").HasColumnType("char(100)");
            entity.Property(e => e.Soyadi).HasColumnName("soyadi").HasColumnType("char(100)");
            entity.Property(e => e.DogumTarihi).HasColumnName("dogumTarihi");
            entity.Property(e => e.DogumYeri).HasColumnName("dogumYeri").HasColumnType("char(100)");
            entity.Property(e => e.Cinsiyeti).HasColumnName("cinsiyeti").HasColumnType("tinyint(1)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
