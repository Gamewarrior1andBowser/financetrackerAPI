using Microsoft.AspNetCore.Mvc;

namespace financetrackerAPI.Controllers;

public class TransactionPageController : Controller
{
    // GET /TransactionPage
    public IActionResult Index()
    {
        return View("~/Views/Transaction/Index.cshtml");

    }

    // GET /TransactionPage/Create
    public IActionResult Create()
    {
        return View("~/Views/Transaction/Create.cshtml");

    }

    // GET /TransactionPage/Edit/{id}
    public IActionResult Edit(int id)
    {
        ViewData["transactionID"] = id;
        return View("~/Views/Transaction/Edit.cshtml");

    }

    // GET /TransactionPage/Details/{id}
    public IActionResult Details(int id)
    {
        ViewData["transactionID"] = id;
        return View("~/Views/Transaction/Details.cshtml");
    }
}
