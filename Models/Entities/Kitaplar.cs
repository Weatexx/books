using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace books.Models.Entities;

public partial class Kitaplar
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }  // Property adı Id olarak kalacak ama veritabanında ID olarak eşlenecek

    [Column("adi")]
    [StringLength(200)]
    [Required]
    public string Adi { get; set; } = "";

    [Column("yazarID")]
    [Required]
    public int YazarId { get; set; }

    [Column("dilID")]
    [Required]
    public int DilId { get; set; }

    [Column("sayfaSayisi")]
    [Required]
    public int SayfaSayisi { get; set; }

    [Column("yayineviID")]
    [Required]
    public int YayineviId { get; set; }

    [Column("ozet")]
    [StringLength(5000)]
    [Required]
    public string Ozet { get; set; } = "";

    [Column("yayinTarihi")]
    [Required]
    public DateTime YayinTarihi { get; set; }

    [Column("resim")]
    [StringLength(50)]
    public string? Resim { get; set; }
}
