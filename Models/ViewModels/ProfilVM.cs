public class ProfilVM
{
    public int Id { get; set; }
    public string KullaniciAdi { get; set; } = "";
    public string Resim { get; set; } = "default.jpg";
    public string Bio { get; set; } = "";
    public string WebSitesi { get; set; } = "";
    public int OkuduguKitapSayisi { get; set; }
    public bool KullaniciTakipEdiliyormu { get; set; }
    public List<KitaplikVM> Kitaplik { get; set; } = new();
    public List<AlintiVM> Alintilar { get; set; } = new();
} 