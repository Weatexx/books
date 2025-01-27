using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using books.Models;  // Sadece bu namespace'i kullanalım
using books.Models.Entities;
using books.Models.ViewModels;

namespace books.Controllers;

public class HomeController : Controller
{
    private readonly books.Models.KitapDbContext db;

    public HomeController(books.Models.KitapDbContext _db)
    {
        db = _db;
    }

    public IActionResult Index()
    {
        List<IndexVM> kitaplar = (from x in db.Kitaplars
                                  orderby x.YayinTarihi descending
                                  select new IndexVM
                                  {
                                      Id = x.Id,
                                      KitapAdi = x.Adi,
                                      Resim = x.Resim,
                                      YayinTarihi = x.YayinTarihi.ToShortDateString()
                                  }).Take(9).ToList();

        return View(kitaplar); // çektiğimiz verileri view'e gönderiyoruz
    }

    [Route("/Kitaplar")]
    //[Route("/Kitaplar/Yazar/{yazarId?}")]
    [Route("/Kitaplar/Tur/{turId?}")]
    public IActionResult Kitaplar(int? yazarId, int? turId)
    {
        List<IndexVM> kitaplar = new List<IndexVM>();

        ViewBag.YazarId = "Gelen Yazar Id: " + yazarId;
        ViewBag.TurId = "Gelen Tür Id: " + turId;

        if (yazarId == null && turId == null)
        {
            //Tüm kitapları getir
            kitaplar = (from x in db.Kitaplars
                        orderby x.YayinTarihi descending
                        select new IndexVM
                        {
                            Id = x.Id,
                            KitapAdi = x.Adi,
                            Resim = x.Resim,
                            YayinTarihi = x.YayinTarihi.ToShortDateString()
                        }).ToList();
            //ViewBag.PageTitle = "Tüm Kitaplar (" + kitaplar.Count() + ")";
            ViewBag.PageTitle = String.Format("Tüm Kitaplar ({0})", kitaplar.Count());
        }

        if (yazarId != null)
        {
            //Yalnızca yazarId'si eşleşen kitapları getir
            kitaplar = (from x in db.Kitaplars
                        where x.YazarId == yazarId
                        orderby x.YayinTarihi descending
                        select new IndexVM
                        {

                            Id = x.Id,
                            KitapAdi = x.Adi,
                            Resim = x.Resim,
                            YayinTarihi = x.YayinTarihi.ToShortDateString()
                        }).ToList();
            var yazar = db.Yazarlars.Find(yazarId);
            string yazarAdi = yazar.Adi + " " + yazar.Soyadi;
            var yazarDogumTarihi = yazar.DogumTarihi;
            var yazarDogumYeri = yazar.DogumYeri;
            var yazarCinsiyet = yazar.Cinsiyeti;
            ViewBag.PageBookCount = kitaplar.Count().ToString();
            ViewBag.yazarAdi = yazarAdi;
            ViewBag.yazarDogumTarihi = yazarDogumTarihi;
            ViewBag.yazarDogumYeri = yazarDogumYeri;
            ViewBag.yazarDogumYeri = yazarDogumYeri;
            ViewBag.yazarCinsiyet = yazarCinsiyet;

            ViewBag.PageTitle = String.Format("{0} yazarına ait kitaplar ({1})", yazarAdi, kitaplar.Count());

        }

        if (turId != null)
        {
            kitaplar = (from x in db.Turlertokitaplars
                        join k in db.Kitaplars on x.KitapId equals k.Id
                        where x.TurId == turId
                        select new IndexVM
                        {

                            Id = k.Id,
                            KitapAdi = k.Adi,
                            Resim = k.Resim,
                            YayinTarihi = k.YayinTarihi.ToShortDateString()
                        }).ToList();

            var tur = db.Turlers.Find(turId);
            string turAdi = tur.TurAdi;
            ViewBag.PageTitle = String.Format("{0} türüne ait kitaplar ({1})", turAdi, kitaplar.Count());
        }

        return View(kitaplar); // çektiğimiz verileri view'e gönderiyoruz
    }

    [Route("/Kitap/{id}")]
    public IActionResult KitapDetay(int id)
    {
        var kitap = (from x in db.Kitaplars
                     where x.Id == id
                     select new BookDetailVM
                     {
                         KitapAdi = x.Adi,
                         YayinTarihi = x.YayinTarihi.ToString("dd.MM.yyyy"),
                         Resim = x.Resim ?? "default.jpg",
                         SayfaSayisi = x.SayfaSayisi,
                         Ozet = x.Ozet,
                         Dil = (from d in db.Dillers
                               where d.Id == x.DilId
                               select d.DilAdi).FirstOrDefault() ?? "",
                         Yazar = new YazarListVM 
                         {
                             id = x.YazarId,
                             adi = (from y in db.Yazarlars
                                   where y.Id == x.YazarId
                                   select y.Adi).FirstOrDefault() ?? "",
                             soyadi = (from y in db.Yazarlars
                                      where y.Id == x.YazarId
                                      select y.Soyadi).FirstOrDefault() ?? ""
                         },
                         Yayinevi = (from y in db.Yayinevleris
                                   where y.Id == x.YayineviId
                                   select y.yayineviAdi).FirstOrDefault() ?? "",
                         KitapTurleri = (from t in db.Turlertokitaplars
                                        join tur in db.Turlers on t.TurId equals tur.Id
                                        where t.KitapId == x.Id
                                        select new TurVM 
                                        { 
                                            Id = tur.Id,
                                            TurAdi = tur.TurAdi 
                                        }).ToList()
                     }).FirstOrDefault();

        if (kitap == null)
        {
            return RedirectToAction("Index");
        }

        return View(kitap);
    }

    [Route("/Yazar/{id}")]
    public IActionResult YazarDetay(int id)
    {
        var yazar = (from x in db.Yazarlars
                     where x.Id == id
                     select new YazarListVM
                     {
                         id = x.Id,
                         adi = x.Adi ?? "",
                         soyadi = x.Soyadi ?? "",
                         dogumTarihi = x.DogumTarihi,
                         dogumYeri = x.DogumYeri ?? "",
                         cinsiyeti = x.Cinsiyeti
                     }).FirstOrDefault();

        if (yazar == null)
        {
            return RedirectToAction("Index");
        }

        return View(yazar);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}