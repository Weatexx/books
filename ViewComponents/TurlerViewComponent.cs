using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using books.Models.Entities;
using books.Models.ViewModels;

namespace books.ViewComponents
{
    public class TurlerViewComponent : ViewComponent
    {
        private readonly KitapDbContext db = new KitapDbContext();
        public TurlerViewComponent(KitapDbContext _db)
        {
            db = _db;
        }

         public async Task<IViewComponentResult> InvokeAsync()
        {
            var turler = (from x in db.Turlers select new TurListVM
            {
                id = x.Id,
                tur= x.TurAdi,
                kitapSayisi = (from k in db.Turlertokitaplars where k.TurId == x.Id select x).Count()
            }).ToList();

            return View(turler);
        }

    }
}