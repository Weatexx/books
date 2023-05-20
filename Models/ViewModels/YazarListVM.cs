namespace books.Models.ViewModels;
public class YazarListVM
{
    public int id { get; set; }

    public string yazarAdi { get; set; }

    public DateOnly dogumTarihi { get; set; }

    public string dogumYeri { get; set; }

    public bool cinsiyeti { get; set; }

    public int kitapSayisi { get; set; }
}