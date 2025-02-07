using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace books.Models.Entities;

[Table("alintilar")]
public class Alintilar
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    public int kitap_id { get; set; }
    public int kullanici_id { get; set; }
    public string alinti_metni { get; set; } = "";
    public int sayfa_no { get; set; }
    public int begeni_sayisi { get; set; }
    public DateTime paylasim_tarihi { get; set; }
}