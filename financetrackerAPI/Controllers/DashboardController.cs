using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace financetrackerAPI.Controllers;

public class DashboardController : Controller
{
    // GET /DashboardPage
    [Authorize]
    public IActionResult Index()
    {
        return View();
    }
}
