using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace financetrackerAPI.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            string filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "demo-data.json"
            );


            if (System.IO.File.Exists(filePath))
            {
                string json = System.IO.File.ReadAllText(filePath);


                ViewBag.DemoData = JsonSerializer.Deserialize<JsonElement>(json);
            }
            else
            {
                ViewBag.DemoData = null;
            }


            return View();
        }
    }
}