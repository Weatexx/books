public class KitaplikVM
{
    public int Id { get; set; }
    public int KitapId { get; set; }
    public string KitapAdi { get; set; } = "";
    public string KitapResim { get; set; } = "default.jpg";
    public string Durum { get; set; } = "";
    public DateTime? BaslamaTarihi { get; set; }
    public DateTime? BitirmeTarihi { get; set; }
    public int OkunanSayfa { get; set; }
} 