using financetrackerAPI.Data;
using financetrackerAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace financetrackerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;

    public UserController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        string? idClaim = User.FindFirst("id")?.Value;

        if (idClaim == null || !int.TryParse(idClaim, out int userID))
        {
            return Unauthorized();
        }

        User? user = await _context.Users
            .FirstOrDefaultAsync(u => u.userID == userID);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(new
        {
            name = user.email.Split('@')[0]
        });
    }
}