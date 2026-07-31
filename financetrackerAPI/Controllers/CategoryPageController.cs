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
    }
}