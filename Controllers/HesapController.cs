using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    public class HesapController : Controller
    {
        [HttpGet]
        public IActionResult Giris()
        {
            return View();
        }

        [HttpGet]
        public IActionResult KayitOl()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Profil()
        {
            return View();
        }
    }
} 