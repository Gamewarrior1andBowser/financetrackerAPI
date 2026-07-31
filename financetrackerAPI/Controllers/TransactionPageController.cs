using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace financetrackerAPI.Controllers
{
    [Authorize]
    public class TransactionPageController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Transaction/Index.cshtml");
        }


    }
}