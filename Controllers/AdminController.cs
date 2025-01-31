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

namespace books.Controllers.Admin
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly books.Models.KitapDbContext db;

        public AdminController(books.Models.KitapDbContext _db)
        {
            db = _db;
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
            return RedirectToAction("Yoneticiler");
        }

        // Kitap methods start here:

        [Route("/Admin/Kitaplar")]
        public IActionResult Kitaplar()
        {
            var kitaplar = (from x in db.Kitaplars
                            select new KitaplarVM
                            {
                                Id = x.Id,
                                Adi = x.Adi,
                                YazarId = x.YazarId,
                                DilId = x.DilId,
                                SayfaSayisi = x.SayfaSayisi,
                                YayineviId = x.YayineviId,
                                Ozet = x.Ozet,
                                YayinTarihi = x.YayinTarihi,
                                Resim = x.Resim ?? "default.jpg"
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

        [HttpPost]
        public async Task<IActionResult> KitapEkle(KitaplarVM model)
        {
            if (ModelState.IsValid)
            {
                string resimAdi = "default.jpg";
                if (model.ResimFile != null && model.ResimFile.Length > 0)
                {
                    string uzanti = Path.GetExtension(model.ResimFile.FileName);
                    resimAdi = Guid.NewGuid().ToString() + uzanti;
                    string klasorYolu = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "book");
                    string dosyaYolu = Path.Combine(klasorYolu, resimAdi);

                    if (!Directory.Exists(klasorYolu))
                    {
                        Directory.CreateDirectory(klasorYolu);
                    }

                    using (var stream = new FileStream(dosyaYolu, FileMode.Create))
                    {
                        await model.ResimFile.CopyToAsync(stream);
                    }
                }

                var yeniKitap = new Kitaplar
                {
                    Adi = model.Adi,
                    YazarId = model.YazarId,
                    DilId = model.DilId,
                    SayfaSayisi = model.SayfaSayisi,
                    YayineviId = model.YayineviId,
                    Ozet = model.Ozet,
                    YayinTarihi = model.YayinTarihi,
                    Resim = resimAdi
                };

                db.Kitaplars.Add(yeniKitap);
                await db.SaveChangesAsync();
                return RedirectToAction("Kitaplar");
            }

            ViewBag.Yazarlar = db.Yazarlars.ToList();
            ViewBag.Diller = db.Dillers.ToList();
            ViewBag.Yayinevleri = db.Yayinevleris.ToList();
            ViewBag.Turler = db.Turlers.ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult KitapDuzenle(int id)
        {
            var kitap = db.Kitaplars.Find(id);
            if (kitap == null)
            {
                return RedirectToAction("Kitaplar");
            }

            var model = new KitaplarVM
            {
                Id = kitap.Id,
                Adi = kitap.Adi ?? "",
                YazarId = kitap.YazarId,
                DilId = kitap.DilId,
                YayineviId = kitap.YayineviId,
                YayinTarihi = kitap.YayinTarihi,
                SayfaSayisi = kitap.SayfaSayisi,
                Ozet = kitap.Ozet ?? "",
                Resim = kitap.Resim ?? "default.jpg"
            };

            // Yazarlar listesini çekelim
            ViewBag.Yazarlar = (from y in db.Yazarlars
                                select new books.Models.AdminViewModels.YazarlarVM
                                {
                                    Id = y.Id,
                                    Adi = y.Adi ?? "",
                                    Soyadi = y.Soyadi ?? "",
                                    DogumTarihi = y.DogumTarihi,
                                    DogumYeri = y.DogumYeri ?? "",
                                    Cinsiyeti = y.Cinsiyeti
                                }).ToList();

            // Diller listesini çekelim
            ViewBag.Diller = (from d in db.Dillers
                              select new DillerVM
                              {
                                  Id = d.Id,
                                  Adi = d.DilAdi ?? ""
                              }).ToList();

            // Yayınevleri listesini çekelim
            ViewBag.Yayinevleri = (from y in db.Yayinevleris
                                   select new YayinevleriVM
                                   {
                                       Id = y.Id,
                                       YayineviAdi = y.yayineviAdi ?? "",
                                       Adres = y.adres ?? "",
                                       Tel = y.tel ?? "",
                                       Sira = y.sira
                                   }).ToList();

            ViewBag.Turler = db.Turlers.ToList();

            var secilenTurler = (from x in db.Turlertokitaplars
                                 where x.KitapId == id
                                 select x.TurId).ToList();

            ViewBag.SecilenTurler = secilenTurler;

            return View(model);
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
                        if (kitap.Resim != "default.jpg" && !string.IsNullOrEmpty(kitap.Resim))
                        {
                            string eskiResimYol = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "book", kitap.Resim);
                            if (System.IO.File.Exists(eskiResimYol))
                            {
                                System.IO.File.Delete(eskiResimYol);
                            }
                        }

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

                    kitap.Adi = model.Adi;
                    kitap.YazarId = model.YazarId;
                    kitap.DilId = model.DilId;
                    kitap.SayfaSayisi = model.SayfaSayisi;
                    kitap.YayineviId = model.YayineviId;
                    kitap.Ozet = model.Ozet;
                    kitap.YayinTarihi = model.YayinTarihi;

                    await db.SaveChangesAsync();
                    return RedirectToAction("Kitaplar");
                }
            }

            ViewBag.Yazarlar = (from x in db.Yazarlars
                                select new books.Models.AdminViewModels.YazarlarVM
                                {
                                    Id = x.Id,
                                    Adi = x.Adi ?? "",
                                    Soyadi = x.Soyadi ?? "",
                                    DogumTarihi = x.DogumTarihi,
                                    DogumYeri = x.DogumYeri ?? "",
                                    Cinsiyeti = x.Cinsiyeti
                                }).ToList();

            ViewBag.Diller = (from d in db.Dillers
                              select new DillerVM
                              {
                                  Id = d.Id,
                                  Adi = d.DilAdi ?? ""
                              }).ToList();

            ViewBag.Yayinevleri = (from y in db.Yayinevleris
                                   select new YayinevleriVM
                                   {
                                       Id = y.Id,
                                       YayineviAdi = y.yayineviAdi ?? "",
                                       Adres = y.adres ?? "",
                                       Tel = y.tel ?? "",
                                       Sira = y.sira
                                   }).ToList();

            ViewBag.Turler = db.Turlers.ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult KitapSil(int id)
        {
            var kitap = (from k in db.Kitaplars
                         join y in db.Yazarlars on k.YazarId equals y.Id
                         join yy in db.Yayinevleris on k.YayineviId equals yy.Id
                         join d in db.Dillers on k.DilId equals d.Id
                         where k.Id == id
                         select new KitaplarVM
                         {
                             Id = k.Id,
                             Adi = k.Adi ?? "",
                             YazarId = k.YazarId,
                             YazarAdi = y.Adi ?? "",
                             YazarSoyadi = y.Soyadi ?? "",
                             YayineviId = k.YayineviId,
                             YayineviAdi = yy.yayineviAdi ?? "",
                             DilId = k.DilId,
                             DilAdi = d.DilAdi ?? "",
                             SayfaSayisi = k.SayfaSayisi,
                             Resim = k.Resim,
                             Sira = k.Sira
                         }).FirstOrDefault();

            if (kitap == null)
                return RedirectToAction("Kitaplar");

            return View(kitap);
        }

        [HttpPost]
        public async Task<IActionResult> KitapSilmeOnay(int id)
        {
            var kitap = await db.Kitaplars.FindAsync(id);
            if (kitap != null)
            {
                if (kitap.Resim != "default.jpg" && !string.IsNullOrEmpty(kitap.Resim))
                {
                    string resimYol = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "book", kitap.Resim);
                    if (System.IO.File.Exists(resimYol))
                    {
                        System.IO.File.Delete(resimYol);
                    }
                }

                db.Kitaplars.Remove(kitap);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Kitaplar");
        }

        // Yazarlar metodları
        [Route("/Admin/Yazarlar")]
        public IActionResult Yazarlar()
        {
            var yazarlar = (from x in db.Yazarlars
                           select new books.Models.AdminViewModels.YazarlarVM
                           {
                               Id = x.Id,
                               Adi = x.Adi ?? string.Empty,
                               Soyadi = x.Soyadi ?? string.Empty, 
                               DogumTarihi = x.DogumTarihi,
                               DogumYeri = x.DogumYeri ?? string.Empty,
                               Cinsiyeti = x.Cinsiyeti
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
                var yeniYazar = new Yazarlar
                {
                    Adi = model.Adi,
                    Soyadi = model.Soyadi,
                    DogumTarihi = model.DogumTarihi,
                    DogumYeri = model.DogumYeri,
                    Cinsiyeti = model.Cinsiyeti
                };

                db.Yazarlars.Add(yeniYazar);
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

            var model = new books.Models.AdminViewModels.YazarlarVM
            {
                Id = yazar.Id,
                Adi = yazar.Adi ?? "",
                Soyadi = yazar.Soyadi ?? "",
                DogumTarihi = yazar.DogumTarihi,
                DogumYeri = yazar.DogumYeri ?? "",
                Cinsiyeti = yazar.Cinsiyeti
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> YazarDuzenle(YazarlarVM model)
        {
            if (ModelState.IsValid)
            {
                var yazar = await db.Yazarlars.FindAsync(model.Id);
                if (yazar != null)
                {
                    yazar.Adi = model.Adi;
                    yazar.Soyadi = model.Soyadi;
                    yazar.DogumTarihi = model.DogumTarihi;
                    yazar.DogumYeri = model.DogumYeri;
                    yazar.Cinsiyeti = model.Cinsiyeti;

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

            var model = new books.Models.AdminViewModels.YazarlarVM
            {
                Id = yazar!.Id,
                Adi = yazar.Adi ?? "",
                Soyadi = yazar.Soyadi ?? "",
                DogumTarihi = yazar.DogumTarihi,
                DogumYeri = yazar.DogumYeri ?? "",
                Cinsiyeti = yazar.Cinsiyeti
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
            var dil = db.Dillers.Find(id);
            if (dil == null)
            {
                return RedirectToAction("Diller");
            }

            ViewBag.DilInfo = dil;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DilDuzenle(DillerVM model)
        {
            if (ModelState.IsValid)
            {
                var dil = await db.Dillers.FindAsync(model.Id);
                if (dil != null)
                {
                    dil.DilAdi = model.Adi;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Diller");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult DilSil(int id)
        {
            var dil = db.Dillers.Find(id);
            if (dil == null)
            {
                return RedirectToAction("Diller");
            }

            ViewBag.DilInfo = dil;
            return View();
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
            var mesajlar = (from x in db.Iletisims
                            orderby x.TarihSaat descending
                            select new IletisimVM
                            {
                                Id = x.Id,
                                Eposta = x.Eposta,
                                Konu = x.Konu,
                                Mesaj = x.Mesaj,
                                TarihSaat = x.TarihSaat,
                                Ip = x.Ip,
                                Goruldu = x.Goruldu
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
    }
}
