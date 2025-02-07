using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace books.Models.Entities;

[Table("kullanici_kitaplik")]
public class KullaniciKitaplik
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    public int kullanici_id { get; set; }
    public int kitap_id { get; set; }
    public string durum { get; set; } = "";
    public DateTime? baslama_tarihi { get; set; }
    public DateTime? bitirme_tarihi { get; set; }
} 