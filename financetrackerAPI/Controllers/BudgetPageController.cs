using Microsoft.AspNetCore.Mvc;

namespace financetrackerAPI.Controllers
{
    public class BudgetPageController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Budget/Index.cshtml");
        }

        public IActionResult Create()
        {
            return View("~/Views/Budget/Create.cshtml");
        }

        public IActionResult Edit()
        {
            return View("~/Views/Budget/Edit.cshtml");
        }

        public IActionResult Details()
        {
            return View("~/Views/Budget/Details.cshtml");
        }
    }
}