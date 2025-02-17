using books.Models;
using books.Models.Entities;
using books.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace books.Controllers
{
    [Route("[controller]")]
    public class KitapController : Controller
    {
        private readonly KitapDbContext _context;

        public KitapController(KitapDbContext context)
        {
            _context = context;
        }

        [Route("YorumEkle")]
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult YorumEkle([FromForm] int kitapId, [FromForm] string yorum, [FromForm] int puan)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                
                // Yeni yorum ekle
                var yeniYorum = new Yorumlar
                {
                    kitap_id = kitapId,
                    kullanici_id = userId,
                    yorum = yorum,
                    puan = puan,
                    tarih = DateTime.Now
                };
                _context.Yorumlar.Add(yeniYorum);
                _context.SaveChanges();

                // Kitabın ortalama puanını güncelle
                var kitap = _context.Kitaplars.Find(kitapId);
                if (kitap != null)
                {
                    var tumYorumlar = _context.Yorumlar.Where(y => y.kitap_id == kitapId);
                    if (tumYorumlar.Any())
                    {
                        var ortalamaPuan = tumYorumlar.Average(y => y.puan ?? 0);
                        kitap.OrtalamaPuan = (decimal)ortalamaPuan;
                        kitap.YorumSayisi = tumYorumlar.Count();
                        _context.SaveChanges();
                    }
                }

                TempData["Success"] = "Değerlendirmeniz başarıyla eklendi.";
                return RedirectToAction("Detay", new { id = kitapId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Değerlendirme eklenirken bir hata oluştu: " + ex.Message;
                return RedirectToAction("Detay", new { id = kitapId });
            }
        }

        [Route("Detay/{id}")]
        [AllowAnonymous]
        public IActionResult Detay(int id)
        {
            var kitap = _context.Kitaplars.Find(id);
            if (kitap == null)
                return NotFound();

            // Yazar bilgisini getir
            var yazar = _context.Yazarlars.Find(kitap.YazarId);
            ViewBag.YazarAdi = yazar != null ? $"{yazar.adi} {yazar.soyadi}" : "";

            // Yayınevi bilgisini getir
            var yayinevi = _context.Yayinevleris.Find(kitap.YayineviId);
            ViewBag.YayineviAdi = yayinevi?.yayineviAdi ?? "";

            // Dil bilgisini getir
            var dil = _context.Dillers.Find(kitap.DilId);
            ViewBag.DilAdi = dil?.DilAdi ?? "";

            // Yorumları getir
            var yorumlar = _context.Yorumlar
                .Where(y => y.kitap_id == id)
                .Join(_context.Kullanicilars,
                    y => y.kullanici_id,
                    k => k.id,
                    (y, k) => new YorumVM
                    {
                        Id = y.id,
                        KitapId = y.kitap_id ?? 0,
                        KullaniciId = y.kullanici_id ?? 0,
                        KullaniciAdi = k.usernames,
                        KullaniciResim = k.resim ?? "default.jpg",
                        Puan = y.puan ?? 0,
                        Yorum = y.yorum ?? "",
                        Tarih = y.tarih ?? DateTime.Now
                    })
                .OrderByDescending(y => y.Tarih)
                .ToList();

            ViewBag.Yorumlar = yorumlar;
            return View(kitap);
        }

        [Route("Degerlendirmeler/{id}")]
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Degerlendirmeler(int id)
        {
            try
            {
                var degerlendirmeler = (from d in _context.Yorumlar
                                       join k in _context.Kullanicilars on d.kullanici_id equals k.id
                                       where d.kitap_id == id
                                       orderby d.tarih descending
                                       select new YorumVM
                                       {
                                           Id = d.id,
                                           KitapId = d.kitap_id ?? 0,
                                           KullaniciId = d.kullanici_id ?? 0,
                                           KullaniciAdi = k.usernames ?? "",
                                           KullaniciResim = k.resim ?? "default.jpg",
                                           Puan = d.puan ?? 0,
                                           Yorum = d.yorum ?? "",
                                           Tarih = d.tarih ?? DateTime.Now
                                       }).ToList();

                return Json(new { success = true, yorumlar = degerlendirmeler });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
} 