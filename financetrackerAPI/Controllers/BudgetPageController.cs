using Microsoft.AspNetCore.Mvc;

namespace financetrackerAPI.Controllers
{
    public class BudgetPageController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Budget/Index.cshtml");
        }
    }
}