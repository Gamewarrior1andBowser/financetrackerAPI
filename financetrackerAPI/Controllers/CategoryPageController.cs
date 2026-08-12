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

<<<<<<< HEAD
    public IActionResult Delete(int id)
    {
        ViewData["categoryID"] = id;

=======
    // GET /CategoryPage/Delete/{id}
    public IActionResult Delete(int id) {
        ViewData["categoryID"] = id;
>>>>>>> 4b9bb91593095df735f73ed6ba24576dbbb4a24f
        return View("~/Views/Category/Delete.cshtml");
    }
}
