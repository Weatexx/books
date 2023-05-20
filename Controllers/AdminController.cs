using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using books.Models;
using books.Models.Entities;
using books.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using books.Models.AdminViewModels;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
namespace books.Controllers.Admin;

[Authorize]
public class AdminController : Controller
{
    private readonly KitapDbContext db = new KitapDbContext(); // dependency injection nesnesi
    public AdminController(KitapDbContext _db) // Dep'i parametre olarak ekledik.
    {
        db = _db; // dependency injection yaptık. 
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
                    select x
                    ).FirstOrDefault();

        if (user != null)
        {
            var claims = new List<Claim>{
                new Claim("user",user.Id.ToString()),
                new Claim("role","admin")
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

    [Route("/Admin/Turler")]


    //turler kısmı
    public IActionResult Turler()
    {
        List<TurlerVM> TurListesi = (from x in db.Turlers
                                     select new TurlerVM
                                     {
                                         Id = x.Id,
                                         Sira = x.Sira,
                                         TurAdi = x.TurAdi
                                     }).ToList();

        return View(TurListesi);
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

        tur.Id = gelendata.Id;
        tur.TurAdi = gelendata.TurAdi;
        tur.Sira = gelendata.Sira;

        db.Turlers.Update(tur);
        await db.SaveChangesAsync();

        return Redirect("/Admin/Turler");
    }


//user kısmı

    [Route("/Admin/Users")]
    public IActionResult Users()
    {
        List<UsersVM> UserListesi = (from x in db.Users
                                     select new UsersVM
                                     {
                                         id = x.Id,
                                         username = x.Username,
                                         password = x.Password
                                     }).ToList();

        return View(UserListesi);
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

        user.Id = gelendata.id;
        user.Username = gelendata.username;
        user.Password = gelendata.password;

        db.Users.Update(user);
        await db.SaveChangesAsync();

        return Redirect("/Admin/Users");
    }
}
