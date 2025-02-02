using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace books.Models.AdminViewModels;

public class KitaplarVM
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Kitap adı zorunludur")]
    public string Adi { get; set; } = "";
    
    [Required(ErrorMessage = "Özet zorunludur")]
    public string Ozet { get; set; } = "";
    
    [Required(ErrorMessage = "Sayfa sayısı zorunludur")]
    public int SayfaSayisi { get; set; }
    
    [Required(ErrorMessage = "Yayın tarihi zorunludur")]
    public DateTime YayinTarihi { get; set; }
    
    [Required(ErrorMessage = "Dil seçimi zorunludur")]
    public int DilId { get; set; }
    public string? DilAdi { get; set; }
    
    [Required(ErrorMessage = "Yazar seçimi zorunludur")]
    public int YazarId { get; set; }
    public string? YazarAdi { get; set; }
    public string? YazarSoyadi { get; set; }
    
    [Required(ErrorMessage = "Yayınevi seçimi zorunludur")]
    public int YayineviId { get; set; }
    public string? YayineviAdi { get; set; }
    
    public string? Resim { get; set; }
    
    public IFormFile? ResimFile { get; set; }

    public KitaplarVM()
    {
        YayinTarihi = DateTime.Now;
    }
}