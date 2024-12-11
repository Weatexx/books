using System.ComponentModel.DataAnnotations;

namespace books.Models.AdminViewModels;

public class KitaplarVM
{
    public int id { get; set; }
    [Required]
    public string adi { get; set; }
    [Required] 
    public int yazarID { get; set; }
    [Required]
    public int dilID { get; set; }
    [Required]
    public int sayfaSayisi { get; set; }
    [Required]
    public int yayineviID { get; set; }
    [Required]
    public string ozet { get; set; }
    [Required]
    public DateOnly yayinTarihi { get; set; }
    [Required]
    public string? resim { get; set; }
}