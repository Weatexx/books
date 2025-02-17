using Microsoft.AspNetCore.Mvc;
using books.Models;
using books.Models.Entities;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace books.Controllers;

public class AccountController : Controller
{
    private readonly books.Models.KitapDbContext db;

    public AccountController(books.Models.KitapDbContext _db)
    {
        db = _db;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(string username, string password, string isim, string soyisim, string telno)
    {
        if (db.Kullanicilars.Any(x => x.usernames == username))
        {
            ViewBag.Error = "Bu kullanıcı adı zaten kullanılıyor!";
            return View();
        }

        var yeniKullanici = new Kullanicilar
        {
            usernames = username,
            passwords = password,
            isim = isim,
            soyisim = soyisim,
            telno = telno,
            resim = "default.jpg"
        };

        db.Kullanicilars.Add(yeniKullanici);
        db.SaveChanges();

        return RedirectToAction("Login");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Profil()
    {
        if (!User.Identity.IsAuthenticated)
            return RedirectToAction("Giris");

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        
        var user = db.Kullanicilars.FirstOrDefault(x => x.id == userId);
        if (user == null)
            return RedirectToAction("Giris");

        // Kitaplık verilerini getir
        ViewBag.Okuduklarim = (from kk in db.KullaniciKitaplik
                              join k in db.Kitaplars on kk.kitap_id equals k.Id
                              where kk.kullanici_id == userId && kk.durum == "Okudum"
                              select new
                              {
                                  KitapAdi = k.Adi,
                                  Resim = k.Resim,
                                  BitirmeTarihi = kk.bitirme_tarihi
                              }).ToList();

        ViewBag.Okuyorum = (from kk in db.KullaniciKitaplik
                           join k in db.Kitaplars on kk.kitap_id equals k.Id
                           where kk.kullanici_id == userId && kk.durum == "Okuyorum"
                           select new
                           {
                               KitapAdi = k.Adi,
                               Resim = k.Resim,
                               BaslamaTarihi = kk.baslama_tarihi
                           }).ToList();

        ViewBag.Okuyacaklarim = (from kk in db.KullaniciKitaplik
                                join k in db.Kitaplars on kk.kitap_id equals k.Id
                                where kk.kullanici_id == userId && kk.durum == "Okuyacağım"
                                select new
                                {
                                    KitapAdi = k.Adi,
                                    Resim = k.Resim
                                }).ToList();

        ViewBag.Favoriler = (from kk in db.KullaniciKitaplik
                            join k in db.Kitaplars on kk.kitap_id equals k.Id
                            where kk.kullanici_id == userId && kk.durum == "Favori"
                            select new
                            {
                                KitapAdi = k.Adi,
                                Resim = k.Resim
                            }).ToList();

        return View(user);
    }

    [HttpPost]
    public IActionResult Profil(IFormFile resim)
    {
        if (!User.Identity.IsAuthenticated)
            return RedirectToAction("Giris");

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var user = db.Kullanicilars.FirstOrDefault(x => x.id == userId);
        
        if (user == null)
            return RedirectToAction("Giris");

        if (resim != null && resim.Length > 0)
        {
            try
            {
                // Dosya uzantısını kontrol et
                var izinliUzantilar = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var uzanti = Path.GetExtension(resim.FileName).ToLowerInvariant();
                
                if (!izinliUzantilar.Contains(uzanti))
                {
                    TempData["Error"] = "Sadece .jpg, .jpeg, .png ve .gif uzantılı dosyalar yüklenebilir.";
                    return RedirectToAction("Profil");
                }

                // Dosya boyutunu kontrol et (5MB)
                if (resim.Length > 5 * 1024 * 1024)
                {
                    TempData["Error"] = "Dosya boyutu 5MB'dan büyük olamaz.";
                    return RedirectToAction("Profil");
                }

                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "users");
                
                // Klasör yoksa oluştur
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Eski resmi sil (default.jpg değilse)
                if (!string.IsNullOrEmpty(user.resim) && user.resim != "default.jpg")
                {
                    var eskiResimYolu = Path.Combine(uploadsFolder, user.resim);
                    if (System.IO.File.Exists(eskiResimYolu))
                    {
                        System.IO.File.Delete(eskiResimYolu);
                    }
                }

                // Yeni resmi kaydet
                string yeniResimAdi = $"user_{userId}_{DateTime.Now.Ticks}{uzanti}";
                string yeniResimYolu = Path.Combine(uploadsFolder, yeniResimAdi);
                
                using (var stream = new FileStream(yeniResimYolu, FileMode.Create))
                {
                    resim.CopyTo(stream);
                }

                // Veritabanını güncelle
                user.resim = yeniResimAdi;
                db.SaveChanges();

                TempData["Success"] = "Profil resminiz başarıyla güncellendi.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Resim yüklenirken bir hata oluştu: " + ex.Message;
            }
        }

        return RedirectToAction("Profil");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ProfilGuncelle(string username, string isim, string soyisim, string telno, string password, string passwordConfirm)
    {
        if (!User.Identity.IsAuthenticated)
            return RedirectToAction("Giris");

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var user = db.Kullanicilars.FirstOrDefault(x => x.id == userId);
        
        if (user == null)
        {
            TempData["Error"] = "Kullanıcı bulunamadı!";
            return RedirectToAction("Profil");
        }

        try
        {
            // Kullanıcı adı kontrolü
            if (user.usernames != username && db.Kullanicilars.Any(x => x.usernames == username))
            {
                TempData["Error"] = "Bu kullanıcı adı zaten kullanılıyor!";
                return RedirectToAction("Profil");
            }

            // Bilgileri güncelle
            user.usernames = username;
            user.isim = isim;
            user.soyisim = soyisim;
            user.telno = telno;

            // Şifre değişikliği varsa
            if (!string.IsNullOrEmpty(password))
            {
                if (password != passwordConfirm)
                {
                    TempData["Error"] = "Şifreler eşleşmiyor!";
                    return RedirectToAction("Profil");
                }
                user.passwords = password;
            }

            db.SaveChanges();

            // Oturum bilgilerini güncelle
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                new Claim("FullName", $"{user.isim} {user.soyisim}"),
                new Claim("ProfileImage", user.resim ?? "default.jpg")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties();

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            TempData["Success"] = "Bilgileriniz başarıyla güncellendi!";
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Güncelleme sırasında bir hata oluştu: " + ex.Message;
        }

        return RedirectToAction("Profil");
    }

    [HttpGet]
    public IActionResult Giris()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Giris(string username, string password)
    {
        var user = db.Kullanicilars.FirstOrDefault(x => x.usernames == username && x.passwords == password);
        if (user != null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                new Claim("FullName", $"{user.isim} {user.soyisim}"),
                new Claim("ProfileImage", user.resim ?? "default.jpg")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties();

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction("Index", "Home");
        }

        ViewBag.Error = "Kullanıcı adı veya şifre hatalı!";
        return View();
    }

    [HttpGet]
    public IActionResult KayitOl()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> KayitOl(string username, string password, string isim, string soyisim, string telno)
    {
        if (db.Kullanicilars.Any(x => x.usernames == username))
        {
            ViewBag.Error = "Bu kullanıcı adı zaten kullanılıyor!";
            return View();
        }

        var yeniKullanici = new Kullanicilar
        {
            usernames = username,
            passwords = password,
            isim = isim,
            soyisim = soyisim,
            telno = telno,
            resim = "default.jpg"
        };

        db.Kullanicilars.Add(yeniKullanici);
        await db.SaveChangesAsync();

        return RedirectToAction("Giris");
    }
} 