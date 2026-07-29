using Microsoft.AspNetCore.Mvc;

namespace financetrackerAPI.Controllers
{
    public class TransactionPageController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Transaction/Index.cshtml");
        }
    }
}