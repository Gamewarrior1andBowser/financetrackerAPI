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

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(User user)
    {
        user.email = user.email.ToLower();

      var existingUser = await _context.Users
       .FirstOrDefaultAsync(u => u.email == user.email);

        if (existingUser != null)
        {
            return BadRequest("Email already exists");
        }

        user.userID = _context.Users.Count();

        user.password =
            BCrypt.Net.BCrypt.HashPassword(user.password);


        user.creationTime = DateTime.Now;
        

        _context.Users.Add(user);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch
        {
         
            return BadRequest("Email already exists");
        }


        return Ok("Registered");
    }

    [AllowAnonymous]

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
            Encoding.UTF8.GetBytes(
                _config["Jwt:Key"]
            ));


        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );


        var claims = new[]
        {
            new Claim(
                "id",
                user.userID.ToString()
            )
        };


        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: credentials
        );


        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}