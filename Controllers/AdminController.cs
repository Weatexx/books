using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using books.Models;
using books.Models.Entities;
using books.Models.AdminViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.IO;
using IletisimVM = books.Models.ViewModels.IletisimVM;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace books.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    [AutoValidateAntiforgeryToken]
    public class AdminController : Controller
    {
        private readonly books.Models.KitapDbContext db;
        private readonly IWebHostEnvironment _env;

        public AdminController(books.Models.KitapDbContext _db, IWebHostEnvironment env)
        {
            db = _db;
            _env = env;
        }

        private void ResetAutoIncrement(string tableName)
        {
            try 
            {
                // Güvenli tablo adı doğrulaması
                var allowedTables = new[] { "kitaplar", "yazarlar", "turler", "diller", "yayinevleri", "users" };
                if (!allowedTables.Contains(tableName.ToLower()))
                {
                    throw new ArgumentException("Geçersiz tablo adı");
                }

                db.Database.ExecuteSqlRaw(
                    "ALTER TABLE {0} AUTO_INCREMENT = 1",
                    tableName);

                db.Database.ExecuteSqlRaw(
                    "SET @count = 0; UPDATE {0} SET id = @count:= @count + 1 ORDER BY id",
                    tableName);
            }
            catch (Exception ex)
            {
                // Hata durumunda loglama yapılabilir
                Console.WriteLine($"ID sıfırlama hatası: {ex.Message}");
            }
        }

        // XSS koruması için HTML encode helper metodu
        private string HtmlEncode(string input)
        {
            return string.IsNullOrEmpty(input) ? "" : System.Web.HttpUtility.HtmlEncode(input);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Login action'ı için kontrol yapmayı atla
            if (context.ActionDescriptor.DisplayName?.Contains("Login") == true)
            {
                base.OnActionExecuting(context);
                return;
            }
            
            // Diğer tüm action'lar için yetki kontrolü yap
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                context.Result = RedirectToAction("Login", "Admin");
                return;
            }
            base.OnActionExecuting(context);
        }

        public IActionResult Index()
        {
            ViewBag.KitapSayisi = db.Kitaplars.Count();
            ViewBag.YazarSayisi = db.Yazarlars.Count();
            ViewBag.YayineviSayisi = db.Yayinevleris.Count();
            ViewBag.TurSayisi = db.Turlers.Count();
            ViewBag.DilSayisi = db.Dillers.Count();
            ViewBag.UserSayisi = db.Users.Count();
            ViewBag.KullaniciSayisi = db.Kullanicilars.Count();

            ViewBag.Kullanicilar = db.Kullanicilars.OrderBy(x => x.id).ToList();

            return View();
        }

        [AllowAnonymous]
        public IActionResult Giris()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Giris(UserVM postedData)
        {
            if (!ModelState.IsValid)
            {
                return View(postedData);
            }

            var user = (from x in db.Users
                        where x.Username == postedData.username && x.Password == postedData.password
                        select x).FirstOrDefault();

            if (user != null)
            {
                var claims = new List<Claim>{
                    new Claim("user", user.Id.ToString()),
                    new Claim("role", "admin")
                };

                var claimsIdendity = new ClaimsIdentity(claims, "Cookies", "user", "role");
                var claimsPrinciple = new ClaimsPrincipal(claimsIdendity);
                await HttpContext.SignInAsync(claimsPrinciple);

                return Redirect("/Admin/Index");
            }
            else
            {
                TempData["NotFound"] = "Böyle bir kullanıcı bulunamadı!";
            }
            return View();
        }

        [Route("/Admin/Logout")]
        public async Task<IActionResult> Signout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/Admin");
        }

        // Turlers methods start here:

        [Route("/Admin/Turler")]
        public IActionResult Turler()
        {
            var turler = (from x in db.Turlers
                          select new TurlerVM
                          {
                              Id = x.Id,
                              TurAdi = x.TurAdi,
                              Sira = x.Sira
                          }).ToList();

            return View(turler);
        }

        [HttpGet]
        public IActionResult TurEkle()
        {
            return View();
        }

        //Tür ekle
        [HttpPost]
        public async Task<IActionResult> TurEkle(TurlerVM gelenData)
        {
            if (!ModelState.IsValid)
            {
                return View(gelenData);
            }
            Turler yeniTur = new Turler
            {
                TurAdi = gelenData.TurAdi,
                Sira = gelenData.Sira
            };

            await db.AddAsync(yeniTur);
            await db.SaveChangesAsync();

            return Redirect("/Admin/Turler");
        }

        //Tur Düzenle
        [HttpGet]
        public IActionResult TurDuzenle(int id)
        {
            ViewBag.UserInfo = (from x in db.Turlers
                                where x.Id == id
                                select new TurlerVM
                                {
                                    Id = x.Id,
                                    TurAdi = x.TurAdi,
                                    Sira = x.Sira
                                }).FirstOrDefault();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TurDuzenle(TurlerVM gelendata)
        {
            var tur = await db.Turlers.FirstOrDefaultAsync(x => x.Id == gelendata.Id);

            if (!ModelState.IsValid)
            {
                return View(gelendata);
            }

            if (tur != null)
            {
                tur.TurAdi = gelendata.TurAdi;
                tur.Sira = gelendata.Sira;
            }

            db.Turlers.Update(tur);
            await db.SaveChangesAsync();

            return Redirect("/Admin/Turler");
        }

        [HttpGet]
        public IActionResult TurSil(int? id)
        {
            ViewBag.TurInfo = (from x in db.Turlers
                               where x.Id == id
                               select new TurlerVM
                               {
                                   Id = x.Id,
                                   TurAdi = x.TurAdi,
                                   Sira = x.Sira
                               }).FirstOrDefault();

            if (id == null)
            {
                return NotFound();
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TurSilmeOnay(int id)
        {
            var tur = await db.Turlers.FindAsync(id);

            if (tur != null)
            {
                db.Turlers.Remove(tur);
            }
            await db.SaveChangesAsync();

            ResetAutoIncrement("turler");

            return RedirectToAction("Turler");
        }

        // User methods start here:

        [Route("/Admin/Yoneticiler")]
        public IActionResult Yoneticiler()
        {
            ViewBag.Title = "Yöneticiler";
            var yoneticiler = (from x in db.Users
                              select new YoneticiVM
                              {
                                  Id = x.Id,
                                  KullaniciAdi = x.Username
                              }).ToList();
            return View(yoneticiler);
        }

        public IActionResult YoneticiEkle()
        {
            return View();
        }

        [HttpPost]
        public IActionResult YoneticiEkle(YoneticiVM model)
        {
            if (db.Users.Any(x => x.Username == model.KullaniciAdi))
            {
                ViewBag.Error = "Bu kullanıcı adı zaten kullanılıyor!";
                return View(model);
            }

            var yeniYonetici = new User
            {
                Username = model.KullaniciAdi,
                Password = model.Sifre
            };

            db.Users.Add(yeniYonetici);
            db.SaveChanges();

            return RedirectToAction("Yoneticiler");
        }

        [HttpGet]
        public IActionResult YoneticiDuzenle(int id)
        {
            var yonetici = db.Users.Find(id);
            if (yonetici == null)
                return RedirectToAction("Yoneticiler");

            var model = new YoneticiVM
            {
                Id = yonetici.Id,
                KullaniciAdi = yonetici.Username
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult YoneticiDuzenle(YoneticiVM model)
        {
            if (db.Users.Any(x => x.Username == model.KullaniciAdi && x.Id != model.Id))
            {
                ViewBag.Error = "Bu kullanıcı adı zaten kullanılıyor!";
                return View(model);
            }

            var yonetici = db.Users.Find(model.Id);
            if (yonetici != null)
            {
                yonetici.Username = model.KullaniciAdi;
                if (!string.IsNullOrEmpty(model.Sifre))
                    yonetici.Password = model.Sifre;

                db.Users.Update(yonetici);
                db.SaveChanges();
            }
            return RedirectToAction("Yoneticiler");
        }

        [HttpGet]
        public IActionResult YoneticiSil(int id)
        {
            var yonetici = db.Users.Find(id);
            if (yonetici == null)
                return RedirectToAction("Yoneticiler");

            var model = new YoneticiVM
            {
                Id = yonetici.Id,
                KullaniciAdi = yonetici.Username
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult YoneticiSilOnay(int id)
        {
            var yonetici = db.Users.Find(id);
            if (yonetici != null)
            {
                db.Users.Remove(yonetici);
                db.SaveChanges();
            }
            ResetAutoIncrement("users");
            return RedirectToAction("Yoneticiler");
        }

        // Kitap methods start here:

        [Route("/Admin/Kitaplar")]
        public IActionResult Kitaplar(string search)
        {
            ViewBag.Search = search;
            var kitaplar = (from k in db.Kitaplars
                           join y in db.Yazarlars on k.YazarId equals y.ID
                           join d in db.Dillers on k.DilId equals d.Id
                           join yy in db.Yayinevleris on k.YayineviId equals yy.Id
                           where string.IsNullOrEmpty(search) || 
                                 k.Adi.Contains(search)
                           select new books.Models.AdminViewModels.KitaplarVM
                           {
                               Id = k.Id,
                               Adi = k.Adi,
                               YazarId = k.YazarId,
                               YazarAdi = y.adi,
                               YazarSoyadi = y.soyadi,
                               DilId = k.DilId,
                               DilAdi = d.DilAdi,
                               SayfaSayisi = k.SayfaSayisi,
                               YayineviId = k.YayineviId,
                               YayineviAdi = yy.yayineviAdi,
                               Ozet = k.Ozet,
                               YayinTarihi = k.YayinTarihi,
                               Resim = k.Resim ?? "default.jpg"
                           }).ToList();

            return View(kitaplar);
        }

        [HttpGet]
        public IActionResult KitapEkle()
        {
            ViewBag.Yazarlar = db.Yazarlars.ToList();
            ViewBag.Diller = db.Dillers.ToList();
            ViewBag.Yayinevleri = db.Yayinevleris.ToList();
            ViewBag.Turler = db.Turlers.ToList();
            return View();
        }

        // Dosya yükleme için güvenli metod
        private async Task<string> SaveFileAsync(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
                return "default.jpg";

            var fileName = Path.GetRandomFileName() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(_env.WebRootPath, folder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }

        [HttpPost]
        public async Task<IActionResult> KitapEkle(KitaplarVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                // Resim yükleme
                model.Resim = await SaveFileAsync(model.ResimFile, "images/book");

                var kitap = new Kitaplar
                {
                    Adi = model.KitapAdi,
                    YazarId = model.YazarId,
                    DilId = model.DilId,
                    SayfaSayisi = model.SayfaSayisi,
                    YayineviId = model.YayineviId,
                    Ozet = model.Ozet,
                    YayinTarihi = model.YayinTarihi,
                    Resim = model.Resim
                };

                await db.Kitaplars.AddAsync(kitap);
                await db.SaveChangesAsync();
                
                TempData["Success"] = "Kitap başarıyla eklendi.";
                return RedirectToAction("Kitaplar");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Bir hata oluştu: " + ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> KitapDuzenle(int id)
        {
            var kitap = await (from k in db.Kitaplars
                              join y in db.Yazarlars on k.YazarId equals y.ID
                              join d in db.Dillers on k.DilId equals d.Id
                              join yy in db.Yayinevleris on k.YayineviId equals yy.Id
                              where k.Id == id
                              select new books.Models.AdminViewModels.KitaplarVM
                              {
                                  Id = k.Id,
                                  Adi = k.Adi,
                                  YazarId = k.YazarId,
                                  YazarAdi = y.adi,
                                  YazarSoyadi = y.soyadi,
                                  DilId = k.DilId,
                                  DilAdi = d.DilAdi,
                                  SayfaSayisi = k.SayfaSayisi,
                                  YayineviId = k.YayineviId,
                                  YayineviAdi = yy.yayineviAdi,
                                  Ozet = k.Ozet,
                                  YayinTarihi = k.YayinTarihi,
                                  Resim = k.Resim ?? "default.jpg"
                              }).FirstOrDefaultAsync();

            if (kitap == null)
                return NotFound();

            ViewBag.Diller = new SelectList(db.Dillers, "Id", "DilAdi");
            ViewBag.Yazarlar = new SelectList(db.Yazarlars, "Id", "Adi");
            ViewBag.Yayinevleri = new SelectList(db.Yayinevleris, "Id", "yayineviAdi");

            return View(kitap);
        }

        [HttpPost]
        public async Task<IActionResult> KitapDuzenle(KitaplarVM model)
        {
            if (ModelState.IsValid)
            {
                var kitap = await db.Kitaplars.FindAsync(model.Id);
                if (kitap != null)
                {
                    if (model.ResimFile != null && model.ResimFile.Length > 0)
                    {
                        // Eski resmi sil
                        if (kitap.Resim != "default.jpg" && !string.IsNullOrEmpty(kitap.Resim))
                        {
                            string eskiResimYol = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "book", kitap.Resim);
                            if (System.IO.File.Exists(eskiResimYol))
                            {
                                System.IO.File.Delete(eskiResimYol);
                            }
                        }

                        // Yeni resmi kaydet
                        string uzanti = Path.GetExtension(model.ResimFile.FileName);
                        string yeniResimAdi = Guid.NewGuid().ToString() + uzanti;
                        string klasorYolu = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "book");
                        string dosyaYolu = Path.Combine(klasorYolu, yeniResimAdi);

                        if (!Directory.Exists(klasorYolu))
                        {
                            Directory.CreateDirectory(klasorYolu);
                        }

                        using (var stream = new FileStream(dosyaYolu, FileMode.Create))
                        {
                            await model.ResimFile.CopyToAsync(stream);
                        }
                        kitap.Resim = yeniResimAdi;
                    }

                    kitap.Adi = model.KitapAdi;
                    kitap.Ozet = model.Ozet;
                    kitap.SayfaSayisi = model.SayfaSayisi;
                    kitap.YayinTarihi = model.YayinTarihi;
                    kitap.DilId = model.DilId;
                    kitap.YazarId = model.YazarId;
                    kitap.YayineviId = model.YayineviId;

                    db.Entry(kitap).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Kitaplar");
                }
            }
            
            // Hata durumunda dropdown listelerini tekrar doldur
            ViewBag.Diller = new SelectList(db.Dillers, "Id", "DilAdi");
            ViewBag.Yazarlar = new SelectList(db.Yazarlars, "Id", "YazarAdi");
            ViewBag.Yayinevleri = new SelectList(db.Yayinevleris, "Id", "yayineviAdi");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> KitapSil(int id)
        {
            var kitap = await (from k in db.Kitaplars
                              join y in db.Yazarlars on k.YazarId equals y.ID
                              join d in db.Dillers on k.DilId equals d.Id
                              join yy in db.Yayinevleris on k.YayineviId equals yy.Id
                              where k.Id == id
                              select new books.Models.AdminViewModels.KitaplarVM
                              {
                                  Id = k.Id,
                                  Adi = k.Adi,
                                  YazarId = k.YazarId,
                                  YazarAdi = y.adi,
                                  YazarSoyadi = y.soyadi,
                                  DilId = k.DilId,
                                  DilAdi = d.DilAdi,
                                  SayfaSayisi = k.SayfaSayisi,
                                  YayineviId = k.YayineviId,
                                  YayineviAdi = yy.yayineviAdi,
                                  Ozet = k.Ozet,
                                  YayinTarihi = k.YayinTarihi,
                                  Resim = k.Resim ?? "default.jpg"
                              }).FirstOrDefaultAsync();

            if (kitap == null)
                return NotFound();

            return View(kitap);
        }

        [HttpPost]
        public async Task<IActionResult> KitapSilOnay(int id)
        {
            var kitap = await db.Kitaplars.FindAsync(id);
            if (kitap != null)
            {
                // Önce kitabın tür ilişkilerini sil
                var turIliskileri = db.Turlertokitaplars.Where(t => t.KitapId == id);
                db.Turlertokitaplars.RemoveRange(turIliskileri);
                
                // Eğer varsayılan resim değilse, resmi sil
                if (kitap.Resim != null && kitap.Resim != "default.jpg")
                {
                    var resimYolu = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "book", kitap.Resim);
                    if (System.IO.File.Exists(resimYolu))
                    {
                        System.IO.File.Delete(resimYolu);
                    }
                }

                // Kitabı sil
                db.Kitaplars.Remove(kitap);
                await db.SaveChangesAsync();

                ResetAutoIncrement("kitaplar");

                return RedirectToAction("Kitaplar");
            }
            return NotFound();
        }

        // Yazarlar metodları
        [Route("/Admin/Yazarlar")]
        public IActionResult Yazarlar(string search)
        {
            ViewBag.Search = search;
            var yazarlar = (from y in db.Yazarlars
                           where y.aktif == true
                           orderby y.sira, y.adi
                           select new YazarlarVM
                           {
                               ID = y.ID,
                               adi = y.adi,
                               soyadi = y.soyadi,
                               dogumTarihi = y.dogumTarihi,
                               dogumYeri = y.dogumYeri,
                               cinsiyeti = y.cinsiyeti,
                               OlumTarihi = y.OlumTarihi,
                               Resim = y.Resim,
                               sira = y.sira,
                               aktif = y.aktif,
                               biyografi = y.biyografi,
                               KitapSayisi = db.Kitaplars.Count(k => k.YazarId == y.ID)
                           }).ToList();

            return View(yazarlar);
        }

        [HttpGet]
        public IActionResult YazarEkle()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> YazarEkle(YazarlarVM model)
        {
            if (ModelState.IsValid)
            {
                var yazar = new Yazarlar
                {
                    adi = model.adi,
                    soyadi = model.soyadi,
                    dogumTarihi = model.dogumTarihi,
                    dogumYeri = model.dogumYeri,
                    cinsiyeti = model.cinsiyeti,
                    aktif = true
                };

                db.Yazarlars.Add(yazar);
                await db.SaveChangesAsync();
                return RedirectToAction("Yazarlar");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult YazarDuzenle(int id)
        {
            var yazar = db.Yazarlars.Find(id);
            if (yazar == null)
            {
                return RedirectToAction("Yazarlar");
            }

            var model = new YazarlarVM
            {
                ID = yazar.ID,
                adi = yazar.adi,
                soyadi = yazar.soyadi,
                dogumTarihi = yazar.dogumTarihi,
                dogumYeri = yazar.dogumYeri,
                cinsiyeti = yazar.cinsiyeti
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> YazarDuzenle(YazarlarVM model)
        {
            if (ModelState.IsValid)
            {
                var yazar = await db.Yazarlars.FindAsync(model.ID);
                if (yazar != null)
                {
                    yazar.adi = model.adi;
                    yazar.soyadi = model.soyadi;
                    yazar.dogumTarihi = model.dogumTarihi;
                    yazar.dogumYeri = model.dogumYeri;
                    yazar.cinsiyeti = model.cinsiyeti;

                    await db.SaveChangesAsync();
                    return RedirectToAction("Yazarlar");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult YazarSil(int id)
        {
            var yazar = db.Yazarlars.Find(id);
            if (yazar == null)
            {
                return RedirectToAction("Yazarlar");
            }

            var model = new YazarlarVM
            {
                ID = yazar.ID,
                adi = yazar.adi,
                soyadi = yazar.soyadi,
                dogumTarihi = yazar.dogumTarihi,
                dogumYeri = yazar.dogumYeri,
                cinsiyeti = yazar.cinsiyeti
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> YazarSilmeOnay(int id)
        {
            var yazar = await db.Yazarlars.FindAsync(id);
            if (yazar != null)
            {
                db.Yazarlars.Remove(yazar);
                await db.SaveChangesAsync();
            }
            ResetAutoIncrement("yazarlar");
            return RedirectToAction("Yazarlar");
        }

        // Diller metodları
        [Route("/Admin/Diller")]
        public IActionResult Diller()
        {
            var diller = (from x in db.Dillers
                          select new DillerVM
                          {
                              Id = x.Id,
                              Adi = x.DilAdi
                          }).ToList();

            return View(diller);
        }

        [HttpGet]
        public IActionResult DilEkle()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DilEkle(DillerVM model)
        {
            if (ModelState.IsValid)
            {
                var yeniDil = new Diller
                {
                    DilAdi = model.Adi
                };

                db.Dillers.Add(yeniDil);
                await db.SaveChangesAsync();
                return RedirectToAction("Diller");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult DilDuzenle(int id)
        {
            var dil = (from x in db.Dillers
                      where x.Id == id
                      select new DillerVM
                      {
                          Id = x.Id,
                          Adi = x.DilAdi ?? ""
                      }).FirstOrDefault();

            if (dil == null)
                return RedirectToAction("Diller");

            return View(dil);
        }

        [HttpPost]
        public IActionResult DilDuzenle(DillerVM model)
        {
            if (ModelState.IsValid)
            {
                var dil = db.Dillers.Find(model.Id);
                if (dil != null)
                {
                    dil.DilAdi = model.Adi;
                    db.SaveChanges();
                    return RedirectToAction("Diller");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult DilSil(int id)
        {
            var dil = (from x in db.Dillers
                      where x.Id == id
                      select new DillerVM
                      {
                          Id = x.Id,
                          Adi = x.DilAdi ?? ""
                      }).FirstOrDefault();

            if (dil == null)
                return RedirectToAction("Diller");

            return View(dil);
        }

        [HttpPost]
        public async Task<IActionResult> DilSilmeOnay(int id)
        {
            var dil = await db.Dillers.FindAsync(id);
            if (dil != null)
            {
                db.Dillers.Remove(dil);
                await db.SaveChangesAsync();
            }
            ResetAutoIncrement("diller");
            return RedirectToAction("Diller");
        }

        // Yayınevleri metodları
        [Route("/Admin/Yayinevleri")]
        public IActionResult Yayinevleri()
        {
            var yayinevleri = (from x in db.Yayinevleris
                               select new YayinevleriVM
                               {
                                   Id = x.Id,
                                   YayineviAdi = x.yayineviAdi.Trim(),
                                   Adres = x.adres,
                                   Tel = x.tel.Trim(),
                                   Sira = x.sira
                               }).OrderBy(x => x.Sira).ToList();
            return View(yayinevleri);
        }

        [HttpGet]
        public IActionResult YayineviEkle()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> YayineviEkle(YayinevleriVM model)
        {
            if (ModelState.IsValid)
            {
                var yeniYayinevi = new Yayinevleri
                {
                    yayineviAdi = model.YayineviAdi,
                    adres = model.Adres,
                    tel = model.Tel,
                    sira = model.Sira
                };

                db.Yayinevleris.Add(yeniYayinevi);
                await db.SaveChangesAsync();
                return RedirectToAction("Yayinevleri");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult YayineviDuzenle(int id)
        {
            var yayinevi = db.Yayinevleris.Find(id);
            if (yayinevi == null)
                return RedirectToAction("Yayinevleri");

            var model = new YayinevleriVM
            {
                Id = yayinevi.Id,
                YayineviAdi = yayinevi.yayineviAdi.Trim(),
                Adres = yayinevi.adres,
                Tel = yayinevi.tel.Trim(),
                Sira = yayinevi.sira
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> YayineviDuzenle(YayinevleriVM model)
        {
            if (ModelState.IsValid)
            {
                var yayinevi = await db.Yayinevleris.FindAsync(model.Id);
                if (yayinevi != null)
                {
                    yayinevi.yayineviAdi = model.YayineviAdi;
                    yayinevi.adres = model.Adres;
                    yayinevi.tel = model.Tel;
                    yayinevi.sira = model.Sira;

                    await db.SaveChangesAsync();
                    return RedirectToAction("Yayinevleri");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult YayineviSil(int id)
        {
            var yayinevi = db.Yayinevleris.Find(id);
            if (yayinevi == null)
                return RedirectToAction("Yayinevleri");

            var model = new YayinevleriVM
            {
                Id = yayinevi.Id,
                YayineviAdi = yayinevi.yayineviAdi.Trim(),
                Adres = yayinevi.adres,
                Tel = yayinevi.tel.Trim(),
                Sira = yayinevi.sira
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> YayineviSilOnay(int id)
        {
            var yayinevi = await db.Yayinevleris.FindAsync(id);
            if (yayinevi != null)
            {
                db.Yayinevleris.Remove(yayinevi);
                await db.SaveChangesAsync();
            }
            ResetAutoIncrement("yayinevleri");
            return RedirectToAction("Yayinevleri");
        }

        [Route("/Admin/Kullanicilar")]
        public IActionResult Kullanicilar()
        {
            var kullanicilar = (from x in db.Kullanicilars
                                select new KullanicilarVM
                                {
                                    Id = x.id,
                                    KullaniciAdi = x.usernames,
                                    Isim = x.isim,
                                    Soyisim = x.soyisim,
                                    TelNo = x.telno,
                                    Resim = x.resim ?? "default.jpg"
                                }).ToList();
            return View(kullanicilar);
        }

        [Route("/Admin/Mesajlar")]
        public IActionResult Mesajlar()
        {
            var mesajlar = (from m in db.Iletisims
                           orderby m.TarihSaat descending
                           select new IletisimVM
                           {
                               Id = m.Id,
                               AdSoyad = m.AdSoyad,
                               Email = m.Email,
                               Konu = m.Konu,
                               Mesaj = m.Mesaj,
                               TarihSaat = m.TarihSaat,
                               Ip = m.Ip,
                               Goruldu = m.Goruldu
                           }).ToList();

            return View(mesajlar);
        }

        [HttpPost]
        public async Task<IActionResult> MesajOkundu(int id)
        {
            var mesaj = await db.Iletisims.FindAsync(id);
            if (mesaj != null)
            {
                mesaj.Goruldu = true;
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Mesajlar");
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("/Admin/Login")]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("/Admin/Login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Kullanıcı adı ve şifre gereklidir.");
                return View();
            }

            var admin = db.Users.FirstOrDefault(x => x.Username == username && x.Password == password);
            if (admin != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), authProperties);

                TempData["Success"] = "Başarıyla giriş yapıldı.";
                return RedirectToAction("Index", "Admin");
            }

            TempData["Error"] = "Kullanıcı adı veya şifre hatalı!";
            return View();
        }

        public IActionResult MesajlariGoruntule()
        {
            try 
            {
                var mesajlar = (from m in db.Iletisims
                               orderby m.TarihSaat descending
                               select new IletisimVM
                               {
                                   Id = m.Id,
                                   AdSoyad = m.AdSoyad,
                                   Email = m.Email,
                                   Konu = m.Konu,
                                   Mesaj = m.Mesaj,
                                   TarihSaat = m.TarihSaat,
                                   Goruldu = m.Goruldu
                               }).Take(5).ToList();  // Son 5 mesajı göster

                return PartialView("_MesajlarPartial", mesajlar);
            }
            catch (Exception)
            {
                // Hata durumunda boş liste döndür
                return PartialView("_MesajlarPartial", new List<IletisimVM>());
            }
        }

        [HttpPost]
        public IActionResult MesajOkunduOlarakIsaretle(int id)
        {
            var mesaj = db.Iletisims.Find(id);
            if (mesaj != null)
            {
                mesaj.Goruldu = true;
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        public IActionResult OkunmamisMesajSayisi()
        {
            var sayi = db.Iletisims.Count(x => !x.Goruldu);
            return Json(new { count = sayi });
        }

        [HttpPost]
        public IActionResult MesajSil(int id)
        {
            try
            {
                var mesaj = db.Iletisims.Find(id);
                if (mesaj != null && mesaj.Goruldu)
                {
                    db.Iletisims.Remove(mesaj);
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return Json(new { success = false, message = "Mesaj okunmadan silinemez!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Mesaj silinirken bir hata oluştu!" });
            }
        }
    }
}
