using Microsoft.AspNetCore.Mvc;

namespace financetrackerAPI.Controllers
{
    public class DemoController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Demo/Index.cshtml");
        }
    }
}