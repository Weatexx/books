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
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(UserVM postedData)
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

        [Route("/Admin/Users")]
        public IActionResult Users()
        {
            var users = (from x in db.Users
                         select new UserVM
                         {
                             id = x.Id,
                             username = x.Username,
                             password = x.Password
                         }).ToList();

            return View(users);
        }

        //user ekle
        [HttpGet]
        public IActionResult UserEkle()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserEkle(UsersVM gelenData)
        {
            if (!ModelState.IsValid)
            {
                return View(gelenData);
            }
            User yeniUser = new User
            {
                Username = gelenData.username,
                Password = gelenData.password,
            };

            await db.AddAsync(yeniUser);
            await db.SaveChangesAsync();

            return Redirect("/Admin/Users");
        }

        //user Düzenle
        [HttpGet]
        public IActionResult userDuzenle(int id)
        {
            ViewBag.UserInfo = (from x in db.Users
                                where x.Id == id
                                select new UsersVM
                                {
                                    id = x.Id,
                                    username = x.Username,
                                    password = x.Password
                                }).FirstOrDefault();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> userDuzenle(UsersVM gelendata)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Id == gelendata.id);

            if (!ModelState.IsValid)
            {
                return View(gelendata);
            }

            if (user != null)
            {
                user.Id = gelendata.id;
                user.Username = gelendata.username;
                user.Password = gelendata.password;
            }

            db.Users.Update(user);
            await db.SaveChangesAsync();

            return Redirect("/Admin/Users");
        }

        [HttpGet]
        public IActionResult UserSil(int? id)
        {
            ViewBag.UserInfo = (from x in db.Users
                                where x.Id == id
                                select new UsersVM
                                {
                                    id = x.Id,
                                    username = x.Username,
                                    password = x.Password
                                }).FirstOrDefault();

            if (id == null)
            {
                return NotFound();
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserSilOnay(int id)
        {
            var user = await db.Users.FindAsync(id);

            if (user != null)
            {
                db.Users.Remove(user);
            }
            await db.SaveChangesAsync();

            return RedirectToAction("Users");
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
                                select new YazarlarVM
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
                                       ID = y.Id,
                                       yayineviAdi = y.yayineviAdi ?? "",
                                       adres = y.adres ?? "",
                                       tel = y.tel ?? "",
                                       sira = y.sira
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

            ViewBag.Yazarlar = db.Yazarlars.ToList();
            ViewBag.Diller = db.Dillers.ToList();
            ViewBag.Yayinevleri = db.Yayinevleris.ToList();
            ViewBag.Turler = db.Turlers.ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult KitapSil(int id)
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
                                select new YazarlarVM
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
                                       ID = y.Id,
                                       yayineviAdi = y.yayineviAdi ?? "",
                                       adres = y.adres ?? "",
                                       tel = y.tel ?? "",
                                       sira = y.sira
                                   }).ToList();

            return View(model);
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
                           select new YazarlarVM
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

            var model = new YazarlarVM
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

            var model = new YazarlarVM
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
                                   ID = x.Id,
                                   yayineviAdi = x.yayineviAdi ?? string.Empty,
                                   adres = x.adres ?? string.Empty,
                                   tel = x.tel ?? string.Empty,
                                   sira = x.sira
                               }).ToList();

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
                    yayineviAdi = model.yayineviAdi,
                    adres = model.adres,
                    tel = model.tel,
                    sira = model.sira
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
            {
                return RedirectToAction("Yayinevleri");
            }

            ViewBag.YayineviInfo = yayinevi;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> YayineviDuzenle(YayinevleriVM model)
        {
            if (ModelState.IsValid)
            {
                var yayinevi = await db.Yayinevleris.FindAsync(model.ID);
                if (yayinevi != null)
                {
                    yayinevi.yayineviAdi = model.yayineviAdi;
                    yayinevi.adres = model.adres;
                    yayinevi.tel = model.tel;
                    yayinevi.sira = model.sira;

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
            {
                return RedirectToAction("Yayinevleri");
            }

            ViewBag.YayineviInfo = yayinevi;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> YayineviSilmeOnay(int id)
        {
            var yayinevi = await db.Yayinevleris.FindAsync(id);
            if (yayinevi != null)
            {
                db.Yayinevleris.Remove(yayinevi);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Yayinevleri");
        }
    }
}
