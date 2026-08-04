using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace financetrackerAPI.Controllers

{
  
    public class ReportsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}