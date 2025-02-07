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
        var kitap = (from k in db.Kitaplars
                     where k.Id == id
                     select new BookDetailVM
                     {
                         KitapId = k.Id,
                         KitapAdi = k.Adi,
                         Resim = k.Resim ?? "default.jpg",
                         YayinTarihi = k.YayinTarihi.ToShortDateString(),
                         SayfaSayisi = k.SayfaSayisi,
                         Ozet = k.Ozet,
                         Dil = (from d in db.Dillers
                               where d.Id == k.DilId
                               select d.DilAdi).FirstOrDefault() ?? "",
                         Yazar = new YazarListVM 
                         {
                             id = k.YazarId,
                             adi = (from y in db.Yazarlars
                                   where y.Id == k.YazarId
                                   select y.Adi).FirstOrDefault() ?? "",
                             soyadi = (from y in db.Yazarlars
                                      where y.Id == k.YazarId
                                      select y.Soyadi).FirstOrDefault() ?? ""
                         },
                         Yayinevi = (from y in db.Yayinevleris
                                   where y.Id == k.YayineviId
                                   select y.yayineviAdi).FirstOrDefault() ?? "",
                         KitapTurleri = (from t in db.Turlertokitaplars
                                        join tur in db.Turlers on t.TurId equals tur.Id
                                        where t.KitapId == k.Id
                                        select new TurVM 
                                        { 
                                            Id = tur.Id,
                                            TurAdi = tur.TurAdi 
                                        }).ToList()
                     }).FirstOrDefault();

        if (kitap == null)
        {
            return NotFound();
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

    public IActionResult Arama(string q)
    {
        if (string.IsNullOrWhiteSpace(q))
            return RedirectToAction("Index");

        var sonuclar = (from k in db.Kitaplars
                       join y in db.Yazarlars on k.YazarId equals y.Id
                       where k.Adi.Contains(q) || 
                             (y.Adi + " " + y.Soyadi).Contains(q)
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

        var viewModel = new ProfilViewModel
        {
            Id = kullanici.id,
            KullaniciAdi = kullanici.usernames ?? "",
            Resim = kullanici.resim ?? "default.jpg",
            Bio = kullanici.bio ?? "",
            WebSitesi = kullanici.web_sitesi ?? "",
            OkuduguKitapSayisi = kullanici.okudugu_kitap_sayisi,
            TakipciSayisi = kullanici.takipci_sayisi,
            TakipEdilenSayisi = kullanici.takip_edilen_sayisi
        };

        // Kullanıcının kitaplığını getir
        viewModel.Kitaplik = (from k in db.KullaniciKitaplik
                             join kitap in db.Kitaplars on k.kitap_id equals kitap.Id
                             where k.kullanici_id == id
                             select new KitaplikViewModel
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
                              select new AlintiViewModel
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
    public IActionResult KitaplikEkle([FromBody] KitaplikEkleViewModel model)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            
            var kitaplik = new KullaniciKitaplik
            {
                kitap_id = model.kitapId,
                kullanici_id = userId,
                durum = model.durum,
                baslama_tarihi = model.durum == "Okuyorum" ? DateTime.Now : null,
                bitirme_tarihi = model.durum == "Okudum" ? DateTime.Now : null
            };

            db.KullaniciKitaplik.Add(kitaplik);
            db.SaveChanges();

            return Json(new { success = true });
        }
        catch (Exception)
        {
            return Json(new { success = false, message = "Kitap eklenirken bir hata oluştu" });
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

    [Authorize]
    [HttpPost]
    public IActionResult DegerlendirmeEkle([FromBody] DegerlendirmeEkleViewModel model)
    {
        try
        {
            Console.WriteLine($"Gelen puan değeri: {model.puan}");
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { success = false, message = "Lütfen önce giriş yapın" });
            }

            if (model == null)
            {
                return Json(new { success = false, message = "Geçersiz veri" });
            }

            // ClaimsPrincipal kontrolü
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Json(new { success = false, message = "Kullanıcı kimliği bulunamadı" });
            }
            var userId = int.Parse(userIdClaim.Value);

            // Model değerlerinin kontrolü
            if (model.kitapId <= 0)
            {
                return Json(new { success = false, message = "Geçersiz kitap ID" });
            }

            if (model.puan < 1 || model.puan > 5)
            {
                return Json(new { success = false, message = "Geçersiz puan değeri" });
            }

            if (string.IsNullOrEmpty(model.yorum))
            {
                return Json(new { success = false, message = "Yorum alanı boş olamaz" });
            }

            // Önceki değerlendirmeyi kontrol et
            var eskiDegerlendirme = db.Yorumlar
                .FirstOrDefault(d => d.kitap_id == model.kitapId && d.kullanici_id == userId);
            
            if (eskiDegerlendirme != null)
            {
                // Varolan değerlendirmeyi güncelle
                eskiDegerlendirme.puan = model.puan;
                eskiDegerlendirme.yorum = model.yorum;
                eskiDegerlendirme.tarih = DateTime.Now;
                Console.WriteLine($"Güncellenen değerlendirme puanı: {eskiDegerlendirme.puan}");
            }
            else
            {
                // Yeni değerlendirme ekle
                var degerlendirme = new Yorumlar
                {
                    kitap_id = model.kitapId,
                    kullanici_id = userId,
                    puan = model.puan,
                    yorum = model.yorum,
                    tarih = DateTime.Now,
                    begeni_sayisi = 0
                };
                Console.WriteLine($"Yeni değerlendirme puanı: {degerlendirme.puan}");
                db.Yorumlar.Add(degerlendirme);
            }

            db.SaveChanges();
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = $"Hata: {ex.Message}" });
        }
    }

    public IActionResult Degerlendirmeler(int id)
    {
        try
        {
            var degerlendirmeler = (from d in db.Yorumlar
                                  join k in db.Kullanicilars on d.kullanici_id equals k.id
                                  where d.kitap_id == id
                                  orderby d.tarih descending
                                  select new DegerlendirmeViewModel
                                  {
                                      Id = d.id,
                                      KullaniciAdi = k.usernames,
                                      KullaniciResim = k.resim,
                                      Puan = d.puan,
                                      Yorum = d.yorum,
                                      Tarih = d.tarih,
                                      BegeniSayisi = d.begeni_sayisi
                                  }).ToList();

            // Debug için puanları kontrol edelim
            foreach (var d in degerlendirmeler)
            {
                Console.WriteLine($"Kullanıcı: {d.KullaniciAdi}, Puan: {d.Puan}");
            }

            return PartialView("~/Views/Home/_Degerlendirmeler.cshtml", degerlendirmeler);
        }
        catch (Exception ex)
        {
            return PartialView("~/Views/Home/_Degerlendirmeler.cshtml", new List<DegerlendirmeViewModel>());
        }
    }
}