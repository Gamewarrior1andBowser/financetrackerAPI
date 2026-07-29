using Microsoft.AspNetCore.Mvc;

namespace financetrackerAPI.Controllers
{
    public class ReportsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}