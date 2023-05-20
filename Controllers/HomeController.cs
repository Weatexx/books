using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using books.Models;
using books.Models.Entities;
using books.Models.ViewModels;

namespace books.Controllers;

public class HomeController : Controller
{

    private readonly KitapDbContext db = new KitapDbContext(); // dependency injection nesnesi
    public HomeController(KitapDbContext _db) // Dep'i parametre olarak ekledik.
    {
        db = _db; // dependency injection yaptık. 
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
        BookDetailVM aktifKitap = (from x in db.Kitaplars
                                   join y in db.Yazarlars on x.YazarId equals y.Id
                                   join k in db.Yayinevleris on x.YayineviId equals k.Id
                                   join d in db.Dillers on x.DilId equals d.Id
                                   let _yazarId = x.YazarId
                                   where x.Id == id
                                   select new BookDetailVM
                                   {
                                       Dil = d.DilAdi,
                                       KitapAdi = x.Adi,
                                       Ozet = x.Ozet,
                                       Resim = x.Resim,
                                       SayfaSayisi = x.SayfaSayisi,
                                       YayinTarihi = x.YayinTarihi.ToShortDateString(),
                                       Yazar = new KitapYazar
                                       {
                                           Id = y.Id,
                                           YazarAdSoyad = y.Adi + " " + y.Soyadi
                                       },
                                       Yayinevi = k.YayineviAdi,
                                       KitapTurleri = (from t in db.Turlertokitaplars
                                                       join m in db.Turlers on t.TurId equals m.Id
                                                       where t.KitapId == x.Id
                                                       select new Turler
                                                       {
                                                           Id = t.TurId,
                                                           TurAdi = m.TurAdi,
                                                           Sira = m.Sira
                                                       }).ToList()

                                   }).FirstOrDefault();
        return View(aktifKitap);
    }

    [Route("/Yazar/{id}")]
    public IActionResult YazarDetay(int id)
    {
        YazarListVM aktifYazar = (from x in db.Yazarlars
                                  where x.Id == id
                                  select new YazarListVM
                                  {
                                      id = x.Id,
                                      yazarAdi = x.Adi + " " + x.Soyadi,
                                      dogumTarihi = x.DogumTarihi,
                                      dogumYeri = x.DogumYeri,
                                      cinsiyeti = x.Cinsiyeti,
                                      kitapSayisi = (from k in db.Kitaplars where k.YazarId == x.Id select x).Count()
                                  }).FirstOrDefault();

        ViewBag.aktifYazar = aktifYazar;


        List<IndexVM> kitaplar = (from y in db.Kitaplars
                                  where y.YazarId == id
                                  orderby y.YayinTarihi descending
                                  select new IndexVM
                                  {

                                      Id = y.Id,
                                      KitapAdi = y.Adi,
                                      Resim = y.Resim,
                                      YayinTarihi = y.YayinTarihi.ToShortDateString()
                                  }).ToList();


        return View(kitaplar);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}