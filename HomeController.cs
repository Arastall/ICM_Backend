using Microsoft.AspNetCore.Mvc;

namespace ICM_2._0
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
