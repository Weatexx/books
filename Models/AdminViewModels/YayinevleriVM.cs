using System.ComponentModel.DataAnnotations;

namespace books.Models.AdminViewModels;

public class YayinevleriVM
{
    public int ID { get; set; }

    [Required(ErrorMessage = "Yayınevi adı zorunludur")]
    public string yayineviAdi { get; set; } = "";

    public string adres { get; set; } = "";

    public string tel { get; set; } = "";

    public int sira { get; set; }

} 