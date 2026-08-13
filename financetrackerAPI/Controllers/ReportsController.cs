using financetrackerAPI.Data;
using Microsoft.AspNetCore.Authorization;
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


        [Authorize]
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
                .Sum(t => t.amount);



            var expenses = _context.Transactions
                .Include(t => t.Category)
                .Where(t => t.Category.Type == "Expense")
                .Sum(t => t.amount);





            var categoryData = _context.Transactions
                .Include(t => t.Category)
                .Where(t => t.Category.Type == "Expense")
                .GroupBy(t => t.Category.name)
                .Select(c => new
                {
                    category = c.Key,

                    amount = c.Sum(x => x.amount)
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