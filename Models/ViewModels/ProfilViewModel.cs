public class ProfilViewModel
{
    public int Id { get; set; }
    public string KullaniciAdi { get; set; } = "";
    public string Resim { get; set; } = "default.jpg";
    public string Bio { get; set; } = "";
    public string WebSitesi { get; set; } = "";
    public int OkuduguKitapSayisi { get; set; }
    public int TakipciSayisi { get; set; }
    public int TakipEdilenSayisi { get; set; }
    public bool KullaniciTakipEdiliyormu { get; set; }
    public List<KitaplikViewModel> Kitaplik { get; set; } = new();
    public List<AlintiViewModel> Alintilar { get; set; } = new();
} 