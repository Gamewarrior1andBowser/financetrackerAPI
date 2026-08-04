using financetrackerAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace financetrackerAPI.Controllers
{
    public class ReportsController : Controller
    {

        private readonly AppDbContext _context;


        public ReportsController(AppDbContext context)
        {
            _context = context;
        }



        public IActionResult Index()
        {
            return View();
        }




        [HttpGet]
        public IActionResult GetReportData()
        {

            var income = _context.Transactions
                .Include(t => t.Category)
                .Where(t => t.Category.Type == "Income")
                .Sum(t => t.Amount);



            var expenses = _context.Transactions
                .Include(t => t.Category)
                .Where(t => t.Category.Type == "Expense")
                .Sum(t => t.Amount);





            var categoryData = _context.Transactions
                .Include(t => t.Category)
                .Where(t => t.Category.Type == "Expense")
                .GroupBy(t => t.Category.Name)
                .Select(c => new
                {
                    category = c.Key,

                    amount = c.Sum(x => x.Amount)
                })
                .ToList();





            return Json(new
            {

                income,

                expenses,

                categories = categoryData

            });

        }

    }
}