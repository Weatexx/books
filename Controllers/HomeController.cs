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

        return View(kitaplar);
    }

    [Route("/Kitaplar")]
    //[Route("/Kitaplar/Yazar/{yazarId?}")]
    [Route("/Kitaplar/Tur/{turId?}")]
    public IActionResult Kitaplar(int? turId)
    {
        List<IndexVM> kitaplar;

        if (turId == null)
        {
            kitaplar = (from x in db.Kitaplars
                        orderby x.YayinTarihi descending
                        select new IndexVM
                        {
                            Id = x.Id,
                            KitapAdi = x.Adi,
                            Resim = x.Resim,
                            YayinTarihi = x.YayinTarihi.ToShortDateString()
                        }).ToList();
        }
        else
        {
            kitaplar = (from x in db.Kitaplars
                        join t in db.Turlertokitaplars on x.Id equals t.KitapId
                        where t.TurId == turId
                        orderby x.YayinTarihi descending
                        select new IndexVM
                        {
                            Id = x.Id,
                            KitapAdi = x.Adi,
                            Resim = x.Resim,
                            YayinTarihi = x.YayinTarihi.ToShortDateString()
                        }).ToList();
        }

        return View(kitaplar);
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

        // Yazarın kitaplarını getir
        ViewBag.YazarinKitaplari = (from x in db.Kitaplars
                                   where x.YazarId == id
                                   select new IndexVM
                                   {
                                       Id = x.Id,
                                       KitapAdi = x.Adi,
                                       Resim = x.Resim,
                                       YayinTarihi = x.YayinTarihi.ToShortDateString()
                                   }).ToList();

        return View(yazar);
    }

    [HttpGet]
    public IActionResult Iletisim()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Iletisim(IletisimVM model)
    {
        if (ModelState.IsValid)
        {
            var yeniMesaj = new Iletisim
            {
                AdSoyad = model.AdSoyad,
                Email = model.Email,
                Konu = model.Konu,
                Mesaj = model.Mesaj,
                TarihSaat = DateTime.Now,
                Ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "",
                Goruldu = false
            };

            db.Iletisims.Add(yeniMesaj);
            db.SaveChanges();

            TempData["MesajGonderildi"] = "Mesajınız başarıyla gönderildi. En kısa sürede mail adresiniz üzerinden size dönüş yapılacaktır.";
            return RedirectToAction("Iletisim");
        }

        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}