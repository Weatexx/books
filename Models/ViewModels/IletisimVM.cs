using System.ComponentModel.DataAnnotations;

namespace books.Models.ViewModels;

public class IletisimVM
{
    public int Id { get; set; }

    [Required(ErrorMessage = "E-posta adresi zorunludur")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
    public string Eposta { get; set; } = "";

    [Required(ErrorMessage = "Konu zorunludur")]
    public string Konu { get; set; } = "";

    [Required(ErrorMessage = "Mesaj zorunludur")]
    public string Mesaj { get; set; } = "";

    public DateTime TarihSaat { get; set; }
    public string Ip { get; set; } = "";
    public bool Goruldu { get; set; }
} 