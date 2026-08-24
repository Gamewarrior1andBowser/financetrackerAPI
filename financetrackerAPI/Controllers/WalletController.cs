using Microsoft.AspNetCore.Authentication.JwtBearer;
using financetrackerAPI.Data;
using financetrackerAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace financetrackerAPI.Controllers
{
    [Route("api/wallet")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class WalletController : ControllerBase
    {
        private readonly AppDbContext _context;
        public WalletController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetWallet()
        {
            string? userIdClaim = User.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }
            if (!int.TryParse(userIdClaim, out int userID))
            {
                return Unauthorized("Invalid user information.");
            }
            Wallet? wallet = await _context.Wallets
                .FirstOrDefaultAsync(w => w.userID == userID);

            if (wallet == null)
            {
                DateTime currentTime = DateTime.UtcNow;
                wallet = new Wallet
                {
                    userID = userID,
                    InitialCash = 100000.00m,
                    AvailableCash = 100000.00m,
                    CreatedAt = currentTime,
                    UpdatedAt = currentTime
                };
                _context.Wallets.Add(wallet);
                await _context.SaveChangesAsync();
            }

            return Ok(new
            {
                walletID = wallet.walletID,
                availableCash = wallet.AvailableCash,
                initialCash = wallet.InitialCash,
                createdAt = wallet.CreatedAt,
                updatedAt = wallet.UpdatedAt
            });
        }
    }
}