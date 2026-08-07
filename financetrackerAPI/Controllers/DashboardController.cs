using Microsoft.AspNetCore.Mvc;

namespace financetrackerAPI.Controllers;

public class DashboardController : Controller
{
    // GET /DashboardPage
    public IActionResult Index()
    {
        return View();
    }
}
