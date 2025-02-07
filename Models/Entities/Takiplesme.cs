using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace books.Models.Entities;

[Table("takiplesme")]
public class Takiplesme
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    public int takip_eden_id { get; set; }
    public int takip_edilen_id { get; set; }
    public DateTime takip_tarihi { get; set; }
} 