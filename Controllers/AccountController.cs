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
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        var user = db.Kullanicilars
            .Select(x => new Kullanicilar
            {
                id = x.id,
                usernames = x.usernames ?? "",
                passwords = x.passwords ?? "",
                isim = x.isim ?? "",
                soyisim = x.soyisim ?? "",
                telno = x.telno,
                resim = string.IsNullOrEmpty(x.resim) ? "default.jpg" : x.resim
            })
            .FirstOrDefault(x => x.usernames == username && x.passwords == password);
        
        if (user != null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties();

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authProperties);

            return RedirectToAction("Index", "Home");
        }

        ViewBag.Error = "Kullanıcı adı veya şifre hatalı!";
        return View();
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
            return RedirectToAction("Login");

        var user = db.Kullanicilars
            .Select(x => new Kullanicilar
            {
                id = x.id,
                usernames = x.usernames ?? "",
                passwords = x.passwords ?? "",
                isim = x.isim ?? "",
                soyisim = x.soyisim ?? "",
                telno = x.telno,
                resim = string.IsNullOrEmpty(x.resim) ? "default.jpg" : x.resim
            })
            .FirstOrDefault(x => x.usernames == User.Identity.Name);

        if (user == null)
        {
            return RedirectToAction("Login");
        }

        return View("Profil", user);
    }

    [HttpPost]
    public IActionResult Profil(IFormFile resim)
    {
        var user = db.Kullanicilars
            .Select(x => new Kullanicilar
            {
                id = x.id,
                usernames = x.usernames ?? "",
                passwords = x.passwords ?? "",
                isim = x.isim ?? "",
                soyisim = x.soyisim ?? "",
                telno = x.telno,
                resim = string.IsNullOrEmpty(x.resim) ? "default.jpg" : x.resim
            })
            .FirstOrDefault(x => x.usernames == User.Identity.Name);
        
        if (user != null && resim != null && resim.Length > 0)
        {
            try
            {
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
                string uzanti = Path.GetExtension(resim.FileName);
                string yeniResimAdi = Guid.NewGuid().ToString() + uzanti;
                string yeniResimYolu = Path.Combine(uploadsFolder, yeniResimAdi);
                
                using (var stream = new FileStream(yeniResimYolu, FileMode.Create))
                {
                    resim.CopyTo(stream);
                }

                // Veritabanını güncelle
                var userToUpdate = db.Kullanicilars.Find(user.id);
                if (userToUpdate != null)
                {
                    userToUpdate.resim = yeniResimAdi;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda işlem yapabilirsiniz
                ViewBag.Error = "Resim yüklenirken bir hata oluştu: " + ex.Message;
            }
        }

        return RedirectToAction("Profil");
    }

    [HttpPost]
    public IActionResult ProfilGuncelle(string username, string isim, string soyisim, string telno, string password, string passwordConfirm)
    {
        var user = db.Kullanicilars.FirstOrDefault(x => x.usernames == User.Identity.Name);
        
        if (user != null)
        {
            // Kullanıcı adı kontrolü
            if (user.usernames != username && db.Kullanicilars.Any(x => x.usernames == username))
            {
                ViewBag.Message = "Bu kullanıcı adı zaten kullanılıyor!";
                ViewBag.Success = false;
                return View("Profil", user);
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
                    ViewBag.Message = "Şifreler eşleşmiyor!";
                    ViewBag.Success = false;
                    return View("Profil", user);
                }
                user.passwords = password;
            }

            db.SaveChanges();
            ViewBag.Message = "Bilgileriniz başarıyla güncellendi!";
            ViewBag.Success = true;
        }

        return RedirectToAction("Profil");
    }
} 