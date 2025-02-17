namespace books.Models.ViewModels
{
    public class YorumVM
    {
        public int Id { get; set; }
        public int KitapId { get; set; }
        public string KitapAdi { get; set; } = "";
        public int KullaniciId { get; set; }
        public string KullaniciAdi { get; set; } = "";
        public string KullaniciResim { get; set; } = "default.jpg";
        public string Yorum { get; set; } = "";
        public int Puan { get; set; }
        public DateTime Tarih { get; set; }
    }
} 