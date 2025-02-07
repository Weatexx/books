public class KitaplikEkleViewModel
{
    public int kitapId { get; set; }
    public string durum { get; set; } = "";
}

public class DegerlendirmeEkleViewModel
{
    public int kitapId { get; set; }
    public int puan { get; set; }
    public string yorum { get; set; } = string.Empty;
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