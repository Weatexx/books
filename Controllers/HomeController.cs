using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using books.Models;  // Sadece bu namespace'i kullanalım
using books.Models.Entities;
using books.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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
        return RedirectToAction("Detay", "Kitap", new { id = id });
    }

    [Route("/Yazar/{id}")]
    public IActionResult YazarDetay(int id)
    {
        var yazar = (from y in db.Yazarlars
                     where y.ID == id && y.aktif == true
                     select new YazarListVM
                     {
                         ID = y.ID,
                         adi = y.adi.Trim(),
                         soyadi = y.soyadi.Trim(),
                         dogumYeri = y.dogumYeri.Trim(),
                         dogumTarihi = y.dogumTarihi,
                         OlumTarihi = y.OlumTarihi,
                         biyografi = y.biyografi ?? "",
                         Resim = y.Resim ?? "default.jpg",
                         cinsiyeti = y.cinsiyeti
                     }).FirstOrDefault();

        if (yazar == null)
            return RedirectToAction("Yazarlar");

        // Yazarın kitaplarını al
        var yazarinKitaplari = (from k in db.Kitaplars
                                where k.YazarId == id
                                select new KitaplarVM
                                {
                                    Id = k.Id,
                                    KitapAdi = k.Adi,
                                    Resim = k.Resim ?? "default.jpg",
                                    YayinTarihi = k.YayinTarihi
                                }).ToList();

        ViewBag.YazarinKitaplari = yazarinKitaplari;
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

    public IActionResult Arama(string q)
    {
        if (string.IsNullOrWhiteSpace(q))
            return RedirectToAction("Index");

        var sonuclar = (from k in db.Kitaplars
                       join y in db.Yazarlars on k.YazarId equals y.ID
                       where k.Adi.Contains(q) || 
                             (y.adi + " " + y.soyadi).Contains(q)
                       select new IndexVM
                       {
                           Id = k.Id,
                           KitapAdi = k.Adi,
                           YayinTarihi = k.YayinTarihi.ToShortDateString(),
                           Resim = k.Resim ?? "default.jpg",
                           SayfaSayisi = k.SayfaSayisi
                       }).ToList();

        ViewBag.PageTitle = $"\"{q}\" için arama sonuçları";
        return View("Kitaplar", sonuclar);
    }

    public IActionResult Profil(int id)
    {
        var kullanici = db.Kullanicilars.Find(id);
        if (kullanici == null)
            return NotFound();
            
        var viewModel = new ProfilVM
        {
            Id = kullanici.id,
            KullaniciAdi = kullanici.usernames ?? "",
            Resim = kullanici.resim ?? "default.jpg",
            Bio = kullanici.bio ?? "",
            WebSitesi = kullanici.web_sitesi ?? "",
            OkuduguKitapSayisi = kullanici.okudugu_kitap_sayisi
        };

        // Kullanıcının kitaplığını getir
        viewModel.Kitaplik = (from k in db.KullaniciKitaplik
                             join kitap in db.Kitaplars on k.kitap_id equals kitap.Id
                             where k.kullanici_id == id
                             select new KitaplikVM
                             {
                                 Id = k.id,
                                 KitapId = kitap.Id,
                                 KitapAdi = kitap.Adi,
                                 KitapResim = kitap.Resim ?? "default.jpg",
                                 Durum = k.durum,
                                 BaslamaTarihi = k.baslama_tarihi,
                                 BitirmeTarihi = k.bitirme_tarihi
                             }).ToList();

        // Kullanıcının alıntılarını getir
        viewModel.Alintilar = (from a in db.Alintilar
                              join kitap in db.Kitaplars on a.kitap_id equals kitap.Id
                              where a.kullanici_id == id
                              orderby a.paylasim_tarihi descending
                              select new AlintiVM
                              {
                                  Id = a.id,
                                  KitapId = kitap.Id,
                                  KitapAdi = kitap.Adi,
                                  AlintiMetni = a.alinti_metni,
                                  SayfaNo = a.sayfa_no,
                                  BegeniSayisi = a.begeni_sayisi,
                                  PaylasimTarihi = a.paylasim_tarihi
                              }).ToList();

        return View(viewModel);
    }

    [Authorize]
    [HttpPost]
    public IActionResult KitaplikEkle([FromBody] KitaplikEkleVM model)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            
            var kitaplik = new KullaniciKitaplik
            {
                kullanici_id = userId,
                kitap_id = model.kitapId.GetValueOrDefault(),
                durum = model.durum ?? "",
                baslama_tarihi = model.durum == "Okuyorum" ? DateTime.Now : null,
                bitirme_tarihi = model.durum == "Okudum" ? DateTime.Now : null
            };

            db.KullaniciKitaplik.Add(kitaplik);
            db.SaveChanges();

            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    [Authorize]
    [HttpPost]
    public IActionResult AlintiEkle([FromBody] Alintilar alinti)
    {
        alinti.kullanici_id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        alinti.paylasim_tarihi = DateTime.Now;
        alinti.begeni_sayisi = 0;

        db.Alintilar.Add(alinti);
        db.SaveChanges();

        return Json(new { success = true });
    }

    public IActionResult Yazarlar()
    {
        var yazarlar = (from y in db.Yazarlars
                        where y.aktif == true
                        orderby y.sira, y.adi
                        select new YazarListVM
                        {
                            ID = y.ID,
                            adi = y.adi.Trim(),
                            soyadi = y.soyadi.Trim(),
                            dogumYeri = y.dogumYeri.Trim(),
                            dogumTarihi = y.dogumTarihi,
                            OlumTarihi = y.OlumTarihi,
                            biyografi = y.biyografi ?? "",
                            Resim = y.Resim ?? "default.jpg",
                            cinsiyeti = y.cinsiyeti,
                            KitapSayisi = db.Kitaplars.Count(k => k.YazarId == y.ID)
                        }).ToList();

        return View(yazarlar);
    }
}