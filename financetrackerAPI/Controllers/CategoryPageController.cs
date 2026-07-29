using Microsoft.AspNetCore.Mvc;

namespace financetrackerAPI.Controllers
{
    public class CategoryPageController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Category/Index.cshtml");
        }
    }
}