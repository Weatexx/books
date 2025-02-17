namespace books.Models.ViewModels
{
    public class KitaplikEkleVM
    {
        public int? kitapId { get; set; }
        public string? durum { get; set; }
    }
}

public class DegerlendirmeEkleViewModel
{
    public int kitapId { get; set; }
    public int puan { get; set; }
    public string? yorum { get; set; }
}

public class DegerlendirmeViewModel
{
    public int Id { get; set; }
    public string KullaniciAdi { get; set; } = "";
    public string KullaniciResim { get; set; } = "";
    public int Puan { get; set; }
    public string Yorum { get; set; } = "";
    public DateTime Tarih { get; set; }
    public int BegeniSayisi { get; set; }
} 