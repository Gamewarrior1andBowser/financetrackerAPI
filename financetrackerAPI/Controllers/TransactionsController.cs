using financetrackerAPI.Data;
using financetrackerAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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


        var categoryExists = await _context.Categories
            .AnyAsync(c =>
                c.categoryID == transaction.categoryID &&
                c.userID == userID);


        if (!categoryExists)
        {
            return BadRequest("Invalid category");
        }


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
            .Include(t => t.Category)
            .Where(t => t.userID == userID)
            .Select(t => new
            {
                t.transactionsID,

                t.amount,

                t.date,

                t.categoryID,


                CategoryName = t.Category != null
                    ? t.Category.Name
                    : "No Category"

            })
            .ToList();



        return Ok(transactions);
    }








    // Get Single Transaction

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var userID = int.Parse(User.FindFirst("id").Value);



        var transaction = _context.Transactions
            .Include(t => t.Category)
            .Where(t =>
                t.transactionsID == id &&
                t.userID == userID)
            .Select(t => new
            {
                t.transactionsID,

                t.amount,

                t.date,

                t.categoryID,


                CategoryName = t.Category != null
                    ? t.Category.Name
                    : "No Category"

            })
            .FirstOrDefault();




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



        transaction.amount = updatedTransaction.amount;

        transaction.categoryID = updatedTransaction.categoryID;

        transaction.date = updatedTransaction.date;



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