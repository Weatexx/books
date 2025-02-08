using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace books.Models.AdminViewModels;

public class YazarlarVM
{
    public int ID { get; set; }

    [Required]
    public string adi { get; set; } = "";

    [Required]
    public string soyadi { get; set; } = "";

    [Required]
    public DateTime dogumTarihi { get; set; }

    [Required]
    public string dogumYeri { get; set; } = "";

    [Required]
    public string cinsiyeti { get; set; } = "E";

    public DateTime? OlumTarihi { get; set; }

    public string? Resim { get; set; } = "default.jpg";

    public int? sira { get; set; } = 0;

    public bool? aktif { get; set; } = true;

    public string? biyografi { get; set; }

    public int KitapSayisi { get; set; }

    public IFormFile? ResimFile { get; set; }

    public YazarlarVM()
    {
        dogumTarihi = DateTime.Now;
    }
} 