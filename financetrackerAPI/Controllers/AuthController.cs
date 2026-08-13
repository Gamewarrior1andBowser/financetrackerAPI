using financetrackerAPI.Data;
using financetrackerAPI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
        else if (string.IsNullOrWhiteSpace(user.password))
        {
            return BadRequest("Password is required");
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
    public async Task<IActionResult> Login(UserLoginRequest login)
    {
        User? user = await _context.Users
            .FirstOrDefaultAsync(u =>
                u.email == login.UsernameOrEmail);

        if (user == null)
        {
            return Unauthorized("This account doesn't exist");
        }

        bool validPassword =
            BCrypt.Net.BCrypt.Verify(
                login.password,
                user.password
            );

        if (!validPassword)
        {
            return Unauthorized("Invalid username/email or password");
        }

        string token = GenerateToken(user);

        List<Claim> claims = new List<Claim>
        {
            new Claim("id", user.userID.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.userID.ToString())
        };

        ClaimsIdentity identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme
        );

        ClaimsPrincipal principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties
            {
                IsPersistent = false,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
            }
        );

        return Ok(new
        {
            token
        });
    }

    private string GenerateToken(User user)
    {
        SymmetricSecurityKey key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
        );

        SigningCredentials credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        Claim[] claims =
        {
            new Claim("id", user.userID.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.userID.ToString())
        };

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}