using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace books.Models.Entities;

[Table("kullanicilar")]
public class Kullanicilar
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }

    [StringLength(50)]
    public string? usernames { get; set; }

    [StringLength(50)]
    public string? passwords { get; set; }

    [StringLength(50)]
    public string? isim { get; set; }

    [StringLength(50)]
    public string? soyisim { get; set; }

    [StringLength(11)]
    public string? telno { get; set; }

    [StringLength(50)]
    public string? resim { get; set; } = "default.jpg";

    [StringLength(50)]
    public string? bio { get; set; }

    [StringLength(50)]
    public string? web_sitesi { get; set; }

    public int okudugu_kitap_sayisi { get; set; }
} 