using financetrackerAPI.Data;
using financetrackerAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace financetrackerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BudgetController : ControllerBase
{
    private readonly AppDbContext _context;

    public BudgetController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var claim = User.FindFirst("id");

        if (claim == null)
            return Unauthorized();

        var userID = int.Parse(claim.Value);

        var budgets = _context.Budgets
            .Where(b => b.userID == userID)
            .ToList();

        return Ok(budgets);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var claim = User.FindFirst("id");

        if (claim == null)
            return Unauthorized();

        var userID = int.Parse(claim.Value);

        var budget = _context.Budgets
            .FirstOrDefault(b =>
                b.budgetID == id &&
                b.userID == userID);

        if (budget == null)
            return NotFound();

        return Ok(budget);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Budget budget)
    {

        var userID = int.Parse(User.FindFirst("id").Value);

        budget.userID = userID;
        budget.date = DateTime.Now;

        if (string.IsNullOrWhiteSpace(budget.username))
            return BadRequest("Budget name is required.");

        if (budget.limits <= 0)
            return BadRequest("Budget limit must be greater than zero.");

        budget.userID = userID;
        budget.date = DateTime.Now;

        _context.Budgets.Add(budget);

        await _context.SaveChangesAsync();

        return Ok(budget);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        Budget updatedBudget)
    {
        var claim = User.FindFirst("id");

        if (claim == null)
            return Unauthorized();

        var userID = int.Parse(claim.Value);

        var budget = _context.Budgets
            .FirstOrDefault(b =>
                b.budgetID == id &&
                b.userID == userID);

        if (budget == null)
            return NotFound();

        budget.username = updatedBudget.username;
        budget.limits = updatedBudget.limits;

        await _context.SaveChangesAsync();

        return Ok(budget);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var claim = User.FindFirst("id");

        if (claim == null)
            return Unauthorized();

        var userID = int.Parse(claim.Value);

        var budget = _context.Budgets
            .FirstOrDefault(b =>
                b.budgetID == id &&
                b.userID == userID);

        if (budget == null)
            return NotFound();

        _context.Budgets.Remove(budget);

        await _context.SaveChangesAsync();

        return Ok();
    }
}