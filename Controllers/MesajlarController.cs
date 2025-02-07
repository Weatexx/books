using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using books.Models.ViewModels;
using books.Models.Entities;

namespace books.Controllers;

[Authorize]
public class MesajlarController : Controller
{
    private readonly books.Models.KitapDbContext db;

    public MesajlarController(books.Models.KitapDbContext _db)
    {
        db = _db;
    }

    public IActionResult Index()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return RedirectToAction("Giris", "Account");
        }
        var userId = int.Parse(userIdClaim.Value);
        var mesajlar = (from m in db.Mesajlar
                       join k in db.Kullanicilars on m.gonderen_id equals k.id
                       where m.alici_id == userId
                       orderby m.tarih descending
                       select new MesajViewModel
                       {
                           Id = m.id,
                           GonderenId = m.gonderen_id,
                           GonderenAdi = k.usernames,
                           GonderenResim = k.resim,
                           Mesaj = m.mesaj,
                           Tarih = m.tarih,
                           Okundu = m.okundu
                       }).ToList();

        return View(mesajlar);
    }

    [HttpPost]
    public IActionResult MesajGonder([FromBody] MesajGonderViewModel model)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Json(new { success = false, message = "Oturum açmanız gerekiyor" });
        }
        var userId = int.Parse(userIdClaim.Value);
        
        var yeniMesaj = new Mesajlar
        {
            gonderen_id = userId,
            alici_id = model.AliciId,
            mesaj = model.Mesaj,
            tarih = DateTime.Now,
            okundu = false
        };

        db.Mesajlar.Add(yeniMesaj);
        db.SaveChanges();

        return Json(new { success = true });
    }
} 