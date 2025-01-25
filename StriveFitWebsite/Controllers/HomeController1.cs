using Microsoft.AspNetCore.Mvc;

namespace StriveFitWebsite.Controllers
{
    public class HomeController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
