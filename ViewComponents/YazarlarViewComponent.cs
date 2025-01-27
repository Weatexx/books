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
                           orderby x.Adi
                           select new YazarlarVM
                           {
                               Id = x.Id,
                               Adi = x.Adi ?? "",
                               Soyadi = x.Soyadi ?? ""
                           }).ToList();

            return View(yazarlar);
        }
    }
}
