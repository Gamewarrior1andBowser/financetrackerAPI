using Microsoft.AspNetCore.Mvc;

namespace financetrackerAPI.Controllers
{
    public class DemoController : Controller
    {
        public IActionResult Transactions()
        {
            return View();
        }


        public IActionResult Categories()
        {
            return View();
        }


        public IActionResult Budget()
        {
            return View();
        }


        public IActionResult Reports()
        {
            return View();
        }
    }
}