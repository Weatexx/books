namespace books.Models.ViewModels;
public class YazarListVM
{
    public int ID { get; set; }
    public string adi { get; set; } = "";
    public string soyadi { get; set; } = "";
    public string dogumYeri { get; set; } = "";
    public DateTime dogumTarihi { get; set; }
    public DateTime? OlumTarihi { get; set; }
    public string? biyografi { get; set; }
    public string? Resim { get; set; } = "default.jpg";
    public string cinsiyeti { get; set; } = "E";
    public int? sira { get; set; } = 0;
    public bool? aktif { get; set; } = true;
    public int KitapSayisi { get; set; }

    public YazarListVM()
    {
        adi = "";
        soyadi = "";
        dogumYeri = "";
    }
}