using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using books.Models;
using books.Models.AdminViewModels;

namespace books.ViewComponents
{
    public class YazarlarViewComponent : ViewComponent
    {
        private readonly KitapDbContext db;

        public YazarlarViewComponent(KitapDbContext _db)
        {
            db = _db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var yazarlar = (from x in db.Yazarlars
                           orderby x.   adi
                           select new YazarlarVM
                           {
                               ID = x.ID,
                               adi = x.adi ?? "",
                               soyadi = x.soyadi ?? ""
                           }).ToList();

            return View(yazarlar);
        }
    }
}
