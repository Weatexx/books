namespace books.Models.AdminViewModels;

public class KullanicilarVM
{
    public int Id { get; set; }
    public string KullaniciAdi { get; set; } = "";
    public string Isim { get; set; } = "";
    public string Soyisim { get; set; } = "";
    public string TelNo { get; set; } = "";
    public string Resim { get; set; } = "default.jpg";
} 