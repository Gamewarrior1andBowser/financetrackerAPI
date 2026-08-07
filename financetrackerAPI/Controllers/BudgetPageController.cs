using Microsoft.AspNetCore.Mvc;

namespace financetrackerAPI.Controllers;

public class BudgetPageController : Controller
{
    public IActionResult Index()
    {
        return View("~/Views/Budget/Index.cshtml");
    }


    public IActionResult Create()
    {
        return View("~/Views/Budget/Create.cshtml");
    }


    public IActionResult Details(int id)
    {
        ViewData["budgetID"] = id;

        return View("~/Views/Budget/Details.cshtml");
    }


    public IActionResult Edit(int id)
    {
        ViewData["budgetID"] = id;

        return View("~/Views/Budget/Edit.cshtml");
    }


    public IActionResult Delete(int id)
    {
        ViewData["budgetID"] = id;

        return View("~/Views/Budget/Delete.cshtml");
    }
}