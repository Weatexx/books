using books.Models.Entities;

namespace books.Models.ViewModels;


class BookDetailVM
{
    public string KitapAdi { get; set; }
    public string YayinTarihi { get; set; }
    public string Resim { get; set; }
    public int SayfaSayisi { get; set; }
    public string Ozet { get; set; }
    public string Dil { get; set; }
    public KitapYazar Yazar { get; set; }
    public string Yayinevi { get; set; }

    public List<Turler> KitapTurleri { get; set; }

}

public class KitapYazar
{
    public int Id { get; set; }
    public string YazarAdSoyad { get; set; }
}

