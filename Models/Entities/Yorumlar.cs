using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace books.Models.Entities;

[Table("yorumlar")]
public class Yorumlar
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    public int kitap_id { get; set; }
    public int kullanici_id { get; set; }
    public string yorum { get; set; } = "";
    public int puan { get; set; }
    public DateTime tarih { get; set; }
    public int begeni_sayisi { get; set; }
} 