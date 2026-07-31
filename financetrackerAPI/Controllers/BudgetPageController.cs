using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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