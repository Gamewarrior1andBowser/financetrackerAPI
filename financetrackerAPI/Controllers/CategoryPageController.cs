using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace financetrackerAPI.Controllers
{
    public class CategoryPageController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Category/Index.cshtml");
        }

        public IActionResult Create()
        {
            return View("~/Views/Category/Create.cshtml");
        }

        public IActionResult Edit(int id)
        {
            ViewBag.categoryID = id;

            return View("~/Views/Category/Edit.cshtml");
        }
    }
}