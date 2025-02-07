public class AlintiViewModel
{
    public int Id { get; set; }
    public int KitapId { get; set; }
    public string KitapAdi { get; set; } = "";
    public int KullaniciId { get; set; }
    public string KullaniciAdi { get; set; } = "";
    public string AlintiMetni { get; set; } = "";
    public int SayfaNo { get; set; }
    public int BegeniSayisi { get; set; }
    public DateTime PaylasimTarihi { get; set; }
    public bool KullanicininBegendimi { get; set; }
} 