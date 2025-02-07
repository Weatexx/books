namespace books.Models.ViewModels;

public class BildirimViewModel
{
    public int Id { get; set; }
    public string Mesaj { get; set; } = "";
    public string Tip { get; set; } = "";
    public int IlgiliIcerikId { get; set; }
    public DateTime Tarih { get; set; }
    public bool Okundu { get; set; }
} 