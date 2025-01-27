namespace books.Models.ViewModels;
public class IndexVM
{
    public int Id { get; set; }
    public string KitapAdi { get; set; } = "";
    public string YayinTarihi { get; set; } = "";
    public string Resim { get; set; } = "default.jpg";
    public int SayfaSayisi { get; set; }
}