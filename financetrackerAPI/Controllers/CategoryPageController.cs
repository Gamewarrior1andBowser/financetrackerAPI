using Microsoft.AspNetCore.Mvc;

namespace financetrackerAPI.Controllers;

public class CategoryPageController : Controller
{
    // GET /CategoryPage
    public IActionResult Index()
    {
        return View("~/Views/Category/Index.cshtml");
    }

    // GET /CategoryPage/Create
    public IActionResult Create()
    {
        return View("~/Views/Category/Create.cshtml");
    }

    // GET /CategoryPage/Edit/{id}
    public IActionResult Edit(int id)
    {
        ViewData["categoryID"] = id;
        return View("~/Views/Category/Edit.cshtml");
    }

    // GET /CategoryPage/Details/{id}
    public IActionResult Details(int id)
    {
        ViewData["categoryID"] = id;
        return View("~/Views/Category/Details.cshtml");
    }

    public IActionResult Delete(int id)
    {
        ViewData["categoryID"] = id;

        return View("~/Views/Category/Delete.cshtml");
    }
}
