using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace books.Models.Entities;

[Table("mesajlar")]
public class Mesajlar
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    public int gonderen_id { get; set; }
    public int alici_id { get; set; }
    public string mesaj { get; set; } = "";
    public DateTime tarih { get; set; }
    public bool okundu { get; set; }
} 