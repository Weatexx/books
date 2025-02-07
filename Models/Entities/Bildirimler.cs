using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace books.Models.Entities;

[Table("bildirimler")]
public class Bildirimler
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    public int kullanici_id { get; set; }
    public string bildirim_metni { get; set; } = "";
    public string bildirim_tipi { get; set; } = ""; // takip, yorum, begeni vs.
    public int ilgili_icerik_id { get; set; }
    public bool okundu { get; set; }
    public DateTime tarih { get; set; }
} 