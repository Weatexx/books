using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using books.Models.Entities;
using books.Models.ViewModels;

namespace books.ViewComponents
{
    public class YazarlarViewComponent : ViewComponent
    {
        private readonly KitapDbContext db = new KitapDbContext();
        public YazarlarViewComponent(KitapDbContext _db)
        {
            db = _db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var yazarlar = (from x in db.Yazarlars
                            select new YazarListVM
                            {
                                id = x.Id,
                                yazarAdi = x.Adi + " " + x.Soyadi,
                                dogumTarihi = x.DogumTarihi,
                                dogumYeri = x.DogumYeri,
                                cinsiyeti = x.Cinsiyeti,
                                kitapSayisi = (from k in db.Kitaplars where k.YazarId == x.Id select x).Count()
                            }).OrderByDescending(x => x.kitapSayisi).Take(10).ToList();

            return View(yazarlar);
        }

    }
}