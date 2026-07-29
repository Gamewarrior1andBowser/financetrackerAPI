using financetrackerAPI.Data;
using financetrackerAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace financetrackerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BudgetController : ControllerBase {
    private readonly AppDbContext _context;

    public BudgetController(AppDbContext context) {
        _context = context;
    }

    // Create Budget
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(Budget budget) {
        var userID = int.Parse(User.FindFirst("id").Value);

        budget.userID = userID;

        _context.Budgets.Add(budget);

        await _context.SaveChangesAsync();

        return Ok(budget);
    }

    // Get All User Budgets
    [Authorize]
    [HttpGet]
    public IActionResult GetAll() {
        var userID = int.Parse(User.FindFirst("id").Value);

        var budget = _context.Budgets
            .Where(t => t.userID == userID)
            .ToList();

        return Ok(budget);
    }

    // Get Single Budget
    [Authorize]
    [HttpGet("{id}")]
    public IActionResult GetById(int id) {
        var userID = int.Parse(User.FindFirst("id").Value);

        var budget = _context.Budgets
            .FirstOrDefault(t =>
                t.budgetID == id &&
                t.userID == userID);


        if (budget == null) {
            return NotFound("Budget not found");
        }


        return Ok(budget);
    }

    // Update Budget
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Budget updatedBudget) {
        var userID = int.Parse(User.FindFirst("id").Value);


        var budget = _context.Budgets
            .FirstOrDefault(t =>
                t.budgetID == id &&
                t.userID == userID);


        if (budget == null) {
            return NotFound("Budget not found");
        }


        budget.Limit = updatedBudget.Limit;

        budget.budgetID = updatedBudget.budgetID;

        budget.Date = updatedBudget.Date;


        await _context.SaveChangesAsync();


        return Ok(budget);
    }

    // Delete Budget
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) {
        var userID = int.Parse(User.FindFirst("id").Value);


        var budget = _context.Budgets
            .FirstOrDefault(t =>
                t.budgetID == id &&
                t.userID == userID);


        if (budget == null) {
            return NotFound("Budget not found");
        }


        _context.Budgets.Remove(budget);

        await _context.SaveChangesAsync();


        return Ok("Budget deleted");
    }
}
