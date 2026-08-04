using Microsoft.AspNetCore.Mvc;

namespace financetrackerAPI.Controllers
{
    public class TransactionPageController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Transaction/Index.cshtml");
        }


        public IActionResult Create()
        {
            return View("~/Views/Transaction/Create.cshtml");
        }


        public IActionResult Edit(int id)
        {
            ViewBag.TransactionID = id;

            return View("~/Views/Transaction/Edit.cshtml");
        }
    }
}