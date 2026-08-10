
using financetrackerAPI.Data;
using financetrackerAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace financetrackerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoryController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoryController(AppDbContext context)
    {
        _context = context;
    }

    // CREATE CATEGORY
    [HttpPost]
    public async Task<IActionResult> Create(Category category)
    {
        var userClaim = User.FindFirst("id");

        if (userClaim == null)
        {
            return Unauthorized("User id not found in token");
        }

        int userID = int.Parse(userClaim.Value);

        if (string.IsNullOrEmpty(category.Name))
        {
            return BadRequest("Category name is required");
        }

        if (string.IsNullOrEmpty(category.Type))
        {
            return BadRequest("Category type is required");
        }

        category.userID = userID;

        _context.Categories.Add(category);

        await _context.SaveChangesAsync();

        return Ok(category);
    }

    // GET ALL USER CATEGORIES
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userClaim = User.FindFirst("id");

        if (userClaim == null)
        {
            return Unauthorized("User id not found in token");
        }

        int userID = int.Parse(userClaim.Value);

        var categories = await _context.Categories
            .Where(c => c.userID == userID)
            .ToListAsync();

        return Ok(categories);
    }

    // GET SINGLE CATEGORY
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var userClaim = User.FindFirst("id");

        if (userClaim == null)
        {
            return Unauthorized("User id not found in token");
        }

        int userID = int.Parse(userClaim.Value);

        var category = await _context.Categories
            .FirstOrDefaultAsync(c =>
                c.categoryID == id &&
                c.userID == userID);

        if (category == null)
        {
            return NotFound("Category not found");
        }

        return Ok(category);
    }

    // UPDATE CATEGORY
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        Category updatedCategory)
    {
        var userClaim = User.FindFirst("id");

        if (userClaim == null)
        {
            return Unauthorized("User id not found in token");
        }

        int userID = int.Parse(userClaim.Value);

        var category = await _context.Categories
            .FirstOrDefaultAsync(c =>
                c.categoryID == id &&
                c.userID == userID);

        if (category == null)
        {
            return NotFound("Category not found");
        }

        category.Name = updatedCategory.Name;
        category.Type = updatedCategory.Type;

        await _context.SaveChangesAsync();

        return Ok(category);
    }

    // DELETE CATEGORY
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userClaim = User.FindFirst("id");

        if (userClaim == null)
        {
            return Unauthorized("User id not found in token");
        }

        int userID = int.Parse(userClaim.Value);

        var category = await _context.Categories
            .FirstOrDefaultAsync(c =>
                c.categoryID == id &&
                c.userID == userID);

        if (category == null)
        {
            return NotFound("Category not found");
        }

        _context.Categories.Remove(category);

        await _context.SaveChangesAsync();

        return Ok("Category deleted");
    }
}

