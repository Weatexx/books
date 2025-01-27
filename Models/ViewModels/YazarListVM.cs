namespace books.Models.ViewModels;
public class YazarListVM
{
    public int id { get; set; }

    public string adi { get; set; }

    public string soyadi { get; set; }

    public DateTime dogumTarihi { get; set; }

    public string dogumYeri { get; set; }

    public bool cinsiyeti { get; set; }

    public int kitapSayisi { get; set; }

    public YazarListVM()
    {
        adi = "";
        soyadi = "";
        dogumYeri = "";
    }
}