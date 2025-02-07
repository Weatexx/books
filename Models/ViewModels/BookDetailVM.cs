using books.Models.Entities;

namespace books.Models.ViewModels;

public class BookDetailVM
{
    public int KitapId { get; set; }
    public string KitapAdi { get; set; } = "";
    public string YayinTarihi { get; set; } = "";
    public string Resim { get; set; } = "";
    public int SayfaSayisi { get; set; }
    public string Ozet { get; set; } = "";
    public string Yayinevi { get; set; } = "";
    public YazarListVM Yazar { get; set; } = new();
    public List<TurVM> KitapTurleri { get; set; } = new();
    public string Dil { get; set; } = "";

    public BookDetailVM()
    {
        KitapAdi = "";
        YayinTarihi = "";
        Resim = "";
        Ozet = "";
        Dil = "";
        Yazar = new YazarListVM();
        Yayinevi = "";
        KitapTurleri = new List<TurVM>();
    }
}

public class TurVM
{
    public int Id { get; set; }
    public string TurAdi { get; set; } = "";
}

public class KitapYazar
{
    public int Id { get; set; }
    public string YazarAdSoyad { get; set; } = "";
}

