using financetrackerAPI.Data;
using financetrackerAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace financetrackerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TransactionsController : ControllerBase
{
    private readonly AppDbContext _context;

    public TransactionsController(AppDbContext context)
    {
        _context = context;
    }


    // Create Transaction
 
    [HttpPost]
    public async Task<IActionResult> Create(Transaction transaction)
    {
        var userID = int.Parse(User.FindFirst("id").Value);

        transaction.userID = userID;

        _context.Transactions.Add(transaction);

        await _context.SaveChangesAsync();

        return Ok(transaction);
    }


    // Get All User Transactions
   
    [HttpGet]
    public IActionResult GetAll()
    {
        var userID = int.Parse(User.FindFirst("id").Value);

        var transactions = _context.Transactions
            .Where(t => t.userID == userID)
            .ToList();

        return Ok(transactions);
    }


    // Get Single Transaction
   
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var userID = int.Parse(User.FindFirst("id").Value);

        var transaction = _context.Transactions
            .FirstOrDefault(t =>
                t.transactionsID == id &&
                t.userID == userID);


        if (transaction == null)
        {
            return NotFound("Transaction not found");
        }


        return Ok(transaction);
    }


    // Update Transaction

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Transaction updatedTransaction)
    {
        var userID = int.Parse(User.FindFirst("id").Value);


        var transaction = _context.Transactions
            .FirstOrDefault(t =>
                t.transactionsID == id &&
                t.userID == userID);


        if (transaction == null)
        {
            return NotFound("Transaction not found");
        }


        transaction.Amount = updatedTransaction.Amount;

        transaction.categoryID = updatedTransaction.categoryID;

        transaction.Date = updatedTransaction.Date;


        await _context.SaveChangesAsync();


        return Ok(transaction);
    }


    // Delete Transaction

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userID = int.Parse(User.FindFirst("id").Value);


        var transaction = _context.Transactions
            .FirstOrDefault(t =>
                t.transactionsID == id &&
                t.userID == userID);


        if (transaction == null)
        {
            return NotFound("Transaction not found");
        }


        _context.Transactions.Remove(transaction);

        await _context.SaveChangesAsync();


        return Ok("Transaction deleted");
    }
}