using financetrackerAPI.Data;
using financetrackerAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace financetrackerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly AppDbContext _context;


    public TransactionsController(AppDbContext context)
    {
        _context = context;
    }



    // Create Transaction
    [Authorize]
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
    [Authorize]
    [HttpGet]
    public IActionResult GetAll()
    {
        var userID = int.Parse(User.FindFirst("id").Value);



        var transactions = _context.Transactions
            .Include(t => t.Category)
            .Where(t => t.userID == userID)
            .Select(t => new
            {
                t.TransactionID,

                t.Amount,

                t.Date,

                t.CategoryID,


                CategoryName = t.Category.Name

            })
            .ToList();



        return Ok(transactions);
    }






    // Get Single Transaction
    [Authorize]
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var userID = int.Parse(User.FindFirst("id").Value);



        var transaction = _context.Transactions
            .Include(t => t.Category)
            .Where(t =>
                t.TransactionID == id &&
                t.userID == userID)
            .Select(t => new
            {
                t.TransactionID,

                t.Amount,

                t.Date,

                t.CategoryID,


                CategoryName = t.Category.Name

            })
            .FirstOrDefault();




        if (transaction == null)
        {
            return NotFound("Transaction not found");
        }



        return Ok(transaction);
    }







    // Update Transaction
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Transaction updatedTransaction)
    {
        var userID = int.Parse(User.FindFirst("id").Value);



        var transaction = _context.Transactions
            .FirstOrDefault(t =>
                t.TransactionID == id &&
                t.userID == userID);



        if (transaction == null)
        {
            return NotFound("Transaction not found");
        }



        transaction.Amount = updatedTransaction.Amount;

        transaction.CategoryID = updatedTransaction.CategoryID;

        transaction.Date = updatedTransaction.Date;



        await _context.SaveChangesAsync();



        return Ok(transaction);
    }







    // Delete Transaction
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userID = int.Parse(User.FindFirst("id").Value);



        var transaction = _context.Transactions
            .FirstOrDefault(t =>
                t.TransactionID == id &&
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