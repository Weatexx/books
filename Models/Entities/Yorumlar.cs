using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace books.Models.Entities;

[Table("yorumlar")]
public class Yorumlar
{
    [Key]
    public int id { get; set; }
    public int? kitap_id { get; set; }
    public int? kullanici_id { get; set; }
    [Column(TypeName = "text")]
    public string? yorum { get; set; }
    [Column(TypeName = "tinyint")]
    public int? puan { get; set; }
    [Column(TypeName = "datetime")]
    public DateTime? tarih { get; set; } = DateTime.Now;
} 