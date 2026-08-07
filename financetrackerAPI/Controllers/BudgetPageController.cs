using Microsoft.AspNetCore.Mvc;

namespace financetrackerAPI.Controllers;

public class BudgetPageController : Controller
{
    // GET /BudgetPage
    public IActionResult Index()
    {
        return View("~/Views/Budget/Index.cshtml");

    }

    // GET /BudgetPage/Details/{id}
    public IActionResult Details(int id)
    {
        ViewData["budgetID"] = id;
        return View("~/Views/Budget/Details.cshtml");

    }
}

