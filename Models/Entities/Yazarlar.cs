using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace books.Models.Entities;

public class Yazarlar
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }

    [Required]
    [StringLength(100)]
    public string adi { get; set; } = "";

    [Required]
    [StringLength(100)]
    public string soyadi { get; set; } = "";

    public DateTime dogumTarihi { get; set; }

    [Required]
    [StringLength(100)]
    public string dogumYeri { get; set; } = "";

    [Required]
    public string cinsiyeti { get; set; } = "E"; // enum 'E','K'

    public string? biyografi { get; set; }

    [StringLength(255)]
    public string? Resim { get; set; } = "default.jpg";

    public DateTime? OlumTarihi { get; set; }

    public int? sira { get; set; } = 0;

    public bool? aktif { get; set; } = true;
}
