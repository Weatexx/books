using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace books.Models.AdminViewModels;

public class KitaplarVM
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Kitap adı zorunludur")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Kitap adı 2-200 karakter arasında olmalıdır")]
    [RegularExpression(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ0-9\s\-_.]+$", ErrorMessage = "Kitap adı geçersiz karakterler içeriyor")]
    public string Adi { get; set; } = "";
    
    [Required(ErrorMessage = "Özet zorunludur")]
    [StringLength(4000, ErrorMessage = "Özet en fazla 4000 karakter olabilir")]
    public string Ozet { get; set; } = "";
    
    [Required(ErrorMessage = "Sayfa sayısı zorunludur")]
    [Range(1, 10000, ErrorMessage = "Sayfa sayısı 1-10000 arasında olmalıdır")]
    public int SayfaSayisi { get; set; }
    
    [Required(ErrorMessage = "Yayın tarihi zorunludur")]
    [DataType(DataType.Date)]
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
    
    [FileExtensions(Extensions = "jpg,jpeg,png,gif", ErrorMessage = "Sadece resim dosyaları yüklenebilir")]
    [MaxFileSize(5 * 1024 * 1024, ErrorMessage = "Dosya boyutu 5MB'dan büyük olamaz")]
    public IFormFile? ResimFile { get; set; }

    public KitaplarVM()
    {
        YayinTarihi = DateTime.Now;
    }
}

// Özel dosya boyutu doğrulama attribute'u
public class MaxFileSizeAttribute : ValidationAttribute
{
    private readonly int _maxFileSize;
    public MaxFileSizeAttribute(int maxFileSize)
    {
        _maxFileSize = maxFileSize;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is IFormFile file)
        {
            if (file.Length > _maxFileSize)
            {
                return new ValidationResult(ErrorMessage);
            }
        }
        return ValidationResult.Success;
    }
}