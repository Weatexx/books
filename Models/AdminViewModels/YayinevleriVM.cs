using System.ComponentModel.DataAnnotations;

namespace books.Models.AdminViewModels;

public class YayinevleriVM
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Yayınevi adı zorunludur")]
    public string YayineviAdi { get; set; } = "";

    public string Adres { get; set; } = "";

    public string Tel { get; set; } = "";

    public int Sira { get; set; }

} 