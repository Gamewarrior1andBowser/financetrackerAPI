using Microsoft.AspNetCore.Mvc;

public class AuthPageController : Controller
{
    public IActionResult Register()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }
}