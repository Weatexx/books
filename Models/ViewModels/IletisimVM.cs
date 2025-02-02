using System.ComponentModel.DataAnnotations;

namespace books.Models.ViewModels;

public class IletisimVM
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Ad Soyad alanı zorunludur")]
    public string AdSoyad { get; set; } = "";

    [Required(ErrorMessage = "E-posta alanı zorunludur")]
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Konu alanı zorunludur")]
    public string Konu { get; set; } = "";

    [Required(ErrorMessage = "Mesaj alanı zorunludur")]
    public string Mesaj { get; set; } = "";

    public DateTime TarihSaat { get; set; }
    public string Ip { get; set; } = "";
    public bool Goruldu { get; set; }
} 