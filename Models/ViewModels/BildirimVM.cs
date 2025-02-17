namespace books.Models.ViewModels;

public class BildirimVM
{
    public int Id { get; set; }
    public string Tip { get; set; } = "";
    public string Mesaj { get; set; } = "";
    public bool Okundu { get; set; }
    public DateTime Tarih { get; set; }
    public int IlgiliIcerikId { get; set; }
} 