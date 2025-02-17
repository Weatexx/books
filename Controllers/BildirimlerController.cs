using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using books.Models.Entities;
using books.Models.ViewModels;

namespace books.Controllers;

[Authorize]
public class BildirimlerController : Controller
{
    private readonly books.Models.KitapDbContext db;

    public BildirimlerController(books.Models.KitapDbContext _db)
    {
        db = _db;
    }

    public IActionResult Index()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var bildirimler = (from b in db.Bildirimler
                          where b.kullanici_id == userId
                          orderby b.tarih descending
                          select new BildirimVM
                          {
                              Id = b.id,
                              Mesaj = b.bildirim_metni,
                              Tip = b.bildirim_tipi,
                              IlgiliIcerikId = b.ilgili_icerik_id,
                              Tarih = b.tarih,
                              Okundu = b.okundu
                          }).ToList();

        return View(bildirimler);
    }

    [HttpPost]
    public IActionResult BildirimOku(int id)
    {
        var bildirim = db.Bildirimler.Find(id);
        if (bildirim != null)
        {
            bildirim.okundu = true;
            db.SaveChanges();
        }
        return Json(new { success = true });
    }
} 