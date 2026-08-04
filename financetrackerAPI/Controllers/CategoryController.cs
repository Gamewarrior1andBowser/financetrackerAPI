using financetrackerAPI.Data;
using financetrackerAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace financetrackerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly AppDbContext _context;


    public CategoryController(AppDbContext context)
    {
        _context = context;
    }



    // Create Category
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(Category category)
    {

        var userClaim = User.FindFirst("id");


        if (userClaim == null)
        {
            return Unauthorized("User id not found in token");
        }


        int userID = int.Parse(userClaim.Value);


        category.userID = userID;


        _context.Categories.Add(category);


        await _context.SaveChangesAsync();


        return Ok(category);
    }




    // Get All Categories
    [Authorize]
    [HttpGet]
    public IActionResult GetAll()
    {

        var userClaim = User.FindFirst("id");


        if (userClaim == null)
        {
            return Unauthorized("User id not found in token");
        }


        int userID = int.Parse(userClaim.Value);



        var categories = _context.Categories
            .Where(c => c.userID == userID)
            .ToList();



        return Ok(categories);
    }




    // Get Category By ID
    [Authorize]
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {

        var userClaim = User.FindFirst("id");


        if (userClaim == null)
        {
            return Unauthorized("User id not found in token");
        }


        int userID = int.Parse(userClaim.Value);



        var category = _context.Categories
            .FirstOrDefault(c =>
                c.CategoryID == id &&
                c.userID == userID);



        if (category == null)
        {
            return NotFound("Category not found");
        }



        return Ok(category);
    }





    // Update Category
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Category updatedCategory)
    {

        var userClaim = User.FindFirst("id");


        if (userClaim == null)
        {
            return Unauthorized("User id not found in token");
        }


        int userID = int.Parse(userClaim.Value);



        var category = _context.Categories
            .FirstOrDefault(c =>
                c.CategoryID == id &&
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





    // Delete Category
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {

        var userClaim = User.FindFirst("id");


        if (userClaim == null)
        {
            return Unauthorized("User id not found in token");
        }


        int userID = int.Parse(userClaim.Value);



        var category = _context.Categories
            .FirstOrDefault(c =>
                c.CategoryID == id &&
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