using financetrackerAPI.Data;
using financetrackerAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace financetrackerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase {
    private readonly AppDbContext _context;

    public CategoryController(AppDbContext context) {
        _context = context;
    }

    // Create Category
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(Category budget) {
        var userId = int.Parse(User.FindFirst("id").Value);

        budget.UserID = userId;

        _context.Categories.Add(budget);

        await _context.SaveChangesAsync();

        return Ok(budget);
    }

    // Get All User Categories
    [Authorize]
    [HttpGet]
    public IActionResult GetAll() {
        var userId = int.Parse(User.FindFirst("id").Value);

        var budget = _context.Categories
            .Where(t => t.UserID == userId)
            .ToList();

        return Ok(budget);
    }

    // Get Single Category
    [Authorize]
    [HttpGet("{id}")]
    public IActionResult GetById(int id) {
        var userId = int.Parse(User.FindFirst("id").Value);

        var budget = _context.Categories
            .FirstOrDefault(t =>
                t.CategoryID == id &&
                t.UserID == userId);


        if (budget == null) {
            return NotFound("Category not found");
        }


        return Ok(budget);
    }

    // Update Category
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Category updatedCategory) {
        var userId = int.Parse(User.FindFirst("id").Value);


        var budget = _context.Categories
            .FirstOrDefault(t =>
                t.CategoryID == id &&
                t.UserID == userId);


        if (budget == null) {
            return NotFound("Category not found");
        }


        budget.Name = updatedCategory.Name;

        budget.CategoryID = updatedCategory.CategoryID;

        budget.Type = updatedCategory.Type;


        await _context.SaveChangesAsync();


        return Ok(budget);
    }

    // Delete Category
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) {
        var userId = int.Parse(User.FindFirst("id").Value);


        var budget = _context.Categories
            .FirstOrDefault(t =>
                t.CategoryID == id &&
                t.UserID == userId);


        if (budget == null) {
            return NotFound("Category not found");
        }


        _context.Categories.Remove(budget);

        await _context.SaveChangesAsync();


        return Ok("Category deleted");
    }
}
