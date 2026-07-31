using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;
namespace financetrackerAPI.Controllers
{

    
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}