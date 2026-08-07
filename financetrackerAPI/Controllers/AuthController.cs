using financetrackerAPI.Data;
using financetrackerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;


namespace financetrackerAPI.Controllers;


[ApiController]
[Route("api/[controller]")]

public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;


    public AuthController(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register(User user)
    {
        if (string.IsNullOrWhiteSpace(user.email))
        {
            return BadRequest("Email is required");
        }
        else if (string.IsNullOrWhiteSpace(user.password)) {
            return BadRequest("Password is required");
        }
        else if (string.IsNullOrWhiteSpace(user.username)) {
            return BadRequest("Username is required");
        }

        user.email = user.email.ToLower().Trim();

        User? existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.email == user.email);

        if (existingUser != null)
        {
            return BadRequest("An account with this email already exists");
        }

        user.password = BCrypt.Net.BCrypt.HashPassword(user.password);

        user.creationTime = DateTime.Now;

        _context.Users.Add(user);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return BadRequest("Unable to create account. Please try again later");
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occurred");
        }

        return Ok("Registration successful");
    }


    [HttpPost("login")]
    public IActionResult Login(User login)
    {
        var user = _context.Users
            .FirstOrDefault(u => u.email == login.email);


        if (user == null)
        {
            return Unauthorized("This account doesn't existy");
        }


        bool validPassword =
            BCrypt.Net.BCrypt.Verify(
                login.password,
                user.password
            );


        if (!validPassword)
        {
            return Unauthorized("Invalid email or password");
        }


        string token = GenerateToken(user);


        return Ok(new
        {
            token
        });
    }



    private string GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"])
        );

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var claims = new[]
        {
        new Claim(ClaimTypes.NameIdentifier, user.userID.ToString())
    };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],     
            audience: _config["Jwt:Audience"],  
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}