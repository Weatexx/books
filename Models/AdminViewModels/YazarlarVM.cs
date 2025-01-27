using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace books.Models.AdminViewModels;

public class YazarlarVM
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Ad alanı zorunludur")]
    public string Adi { get; set; } = "";

    [Required(ErrorMessage = "Soyad alanı zorunludur")]
    public string Soyadi { get; set; } = "";

    [Required(ErrorMessage = "Doğum tarihi alanı zorunludur")]
    public DateTime DogumTarihi { get; set; }

    [Required(ErrorMessage = "Doğum yeri alanı zorunludur")]
    public string DogumYeri { get; set; } = "";

    [Required(ErrorMessage = "Cinsiyet alanı zorunludur")]
    public bool Cinsiyeti { get; set; }

    public string Resim { get; set; } = "default.jpg";

    public IFormFile? ResimFile { get; set; }

    public YazarlarVM()
    {
        DogumTarihi = DateTime.Now;
    }
} 