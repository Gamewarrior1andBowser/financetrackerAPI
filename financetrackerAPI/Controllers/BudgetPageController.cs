using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;



namespace financetrackerAPI.Controllers
{
    [Authorize]
    public class BudgetPageController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Budget/Index.cshtml");
        }
        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "AuthPage");
            }

            return View();
        }
    }
}