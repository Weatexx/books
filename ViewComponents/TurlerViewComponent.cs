using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using books.Models;
using books.Models.ViewModels;

namespace books.ViewComponents
{
    public class TurlerViewComponent : ViewComponent
    {
        private readonly KitapDbContext db;

        public TurlerViewComponent(KitapDbContext _db)
        {
            db = _db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var turler = (from x in db.Turlers
                         select new TurListVM
                         {
                             Id = x.Id,
                             TurAdi = x.TurAdi
                         }).ToList();

            return View(turler);
        }
    }
}