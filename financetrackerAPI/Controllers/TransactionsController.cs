using financetrackerAPI.Data;
using financetrackerAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace financetrackerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class TransactionsController : ControllerBase
{
    private readonly AppDbContext _context;

    public TransactionsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create(Transaction transaction)
    {
        var userID = int.Parse(User.FindFirst("id")!.Value);

        var categoryExists = await _context.Categories.AnyAsync(c =>
            c.categoryID == transaction.categoryID &&
            c.userID == userID);

        if (!categoryExists)
            return BadRequest("Invalid category");

        transaction.userID = userID;

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();

        return Ok(transaction);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userID = int.Parse(User.FindFirst("id")!.Value);

        var transactions = await _context.Transactions
            .Include(t => t.Category)
            .Where(t => t.userID == userID)
            .Select(t => new
            {
                t.transactionsID,
                t.amount,
                t.categoryID,
                t.type,
                t.date,
                t.notes,
                CategoryName = t.Category != null
                    ? t.Category.name
                    : "No Category"
            })
            .ToListAsync();

        return Ok(transactions);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var userID = int.Parse(User.FindFirst("id")!.Value);

        var transaction = await _context.Transactions
            .Include(t => t.Category)
            .Where(t =>
                t.transactionsID == id &&
                t.userID == userID)
            .Select(t => new
            {
                t.transactionsID,
                t.amount,
                t.categoryID,
                t.type,
                t.date,
                t.notes,
                CategoryName = t.Category != null
                    ? t.Category.name
                    : "No Category"
            })
            .FirstOrDefaultAsync();

        if (transaction == null)
            return NotFound("Transaction not found");

        return Ok(transaction);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        Transaction updatedTransaction)
    {
        var userID = int.Parse(User.FindFirst("id")!.Value);

        var transaction = await _context.Transactions
            .FirstOrDefaultAsync(t =>
                t.transactionsID == id &&
                t.userID == userID);

        if (transaction == null)
            return NotFound("Transaction not found");

        transaction.amount = updatedTransaction.amount;
        transaction.categoryID = updatedTransaction.categoryID;
        transaction.type = updatedTransaction.type;
        transaction.date = updatedTransaction.date;
        transaction.notes = updatedTransaction.notes;

        await _context.SaveChangesAsync();

        return Ok(transaction);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userID = int.Parse(User.FindFirst("id")!.Value);

        var transaction = await _context.Transactions
            .FirstOrDefaultAsync(t =>
                t.transactionsID == id &&
                t.userID == userID);

        if (transaction == null)
            return NotFound("Transaction not found");

        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync();

        return Ok("Transaction deleted");
    }
}