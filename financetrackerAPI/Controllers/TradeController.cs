using Microsoft.AspNetCore.Authentication.JwtBearer;
using financetrackerAPI.Data;
using financetrackerAPI.DTOs;
using financetrackerAPI.Models;
using financetrackerAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace financetrackerAPI.Controllers
{
    [Route("api/trades")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TradeController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly MarketDataService _marketDataService;
        public TradeController(AppDbContext context, MarketDataService marketDataService)
        {
            _context = context;
            _marketDataService = marketDataService;
        }

        [HttpPost("buy")]
        public async Task<IActionResult> Buy(TradeRequest request)
        {
            string? userIdClaim = User.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }

            if (!int.TryParse(userIdClaim, out int userID))
            {
                return Unauthorized("Invalid user.");
            }

            if (string.IsNullOrWhiteSpace(request.Symbol))
            {
                return BadRequest("Symbol is required.");
            }

            if (request.Quantity <= 0)
            {
                return BadRequest("Quantity must be greater than zero.");
            }
            string symbol = request.Symbol.Trim().ToUpper();

            // Get live price first
            decimal? livePrice = await _marketDataService.GetLivePriceAsync(symbol);

            if (livePrice == null)
            {
                return BadRequest("Market price is unavailable or symbol is invalid.");
            }

            decimal executionPrice = Math.Round(livePrice.Value, 2);

            // Check if asset already exists
            Asset? asset = await _context.Assets.FirstOrDefaultAsync(a => a.Symbol == symbol);

            // If not, automatically create the asset
            if (asset == null)
            {
                asset = new Asset
                {
                    Symbol = symbol,
                    Name = symbol,
                    AssetType = "Stock",
                    CurrentPrice = executionPrice,
                    ExternalApiID = symbol,
                    LastUpdatedAt = DateTime.UtcNow
                };

                _context.Assets.Add(asset);
                await _context.SaveChangesAsync();
            }
            else
            {
                asset.CurrentPrice = executionPrice;
                asset.LastUpdatedAt = DateTime.UtcNow;
            }

            decimal totalValue = Math.Round(executionPrice * request.Quantity, 2);
            Wallet? wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.userID == userID);

            if (wallet == null)
            {
                return BadRequest("Wallet not found.");
            }

            if (wallet.AvailableCash < totalValue)
            {
                return BadRequest("Not enough cash.");
            }

            PortfolioHolding? holding = await _context.PortfolioHoldings
                .FirstOrDefaultAsync(p =>
                    p.userID == userID &&
                    p.assetID == asset.assetID);

            if (holding == null)
            {
                holding = new PortfolioHolding
                {
                    userID = userID,
                    assetID = asset.assetID,
                    Quantity = request.Quantity,
                    AveragePurchasePrice = executionPrice,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.PortfolioHoldings.Add(holding);
            }
            else
            {
                decimal oldCost = holding.Quantity * holding.AveragePurchasePrice;
                decimal newCost = request.Quantity * executionPrice;
                decimal newQuantity = holding.Quantity + request.Quantity;
                holding.AveragePurchasePrice = Math.Round((oldCost + newCost) / newQuantity, 2);
                holding.Quantity = newQuantity;
                holding.UpdatedAt = DateTime.UtcNow;
            }

            wallet.AvailableCash = Math.Round(wallet.AvailableCash - totalValue, 2);
            wallet.UpdatedAt = DateTime.UtcNow;

            TradeLedger trade = new TradeLedger
            {
                userID = userID,
                assetID = asset.assetID,
                TradeType = "BUY",
                Quantity = request.Quantity,
                ExecutionPrice = executionPrice,
                TotalValue = totalValue,
                CreatedAt = DateTime.UtcNow
            };

            _context.TradeLedgers.Add(trade);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Buy completed.",
                symbol = symbol,
                boughtQuantity = request.Quantity,
                holdingQuantity = holding.Quantity,
                price = executionPrice,
                totalValue = totalValue,
                remainingCash = wallet.AvailableCash
            });
        }

        [HttpPost("sell")]
        public async Task<IActionResult> Sell(TradeRequest request)
        {
            string? userIdClaim = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }

            if (!int.TryParse(userIdClaim, out int userID))
            {
                return Unauthorized("Invalid user.");
            }

            if (string.IsNullOrWhiteSpace(request.Symbol))
            {
                return BadRequest("Symbol is required.");
            }

            if (request.Quantity <= 0)
            {
                return BadRequest("Quantity must be greater than zero.");
            }

            string symbol = request.Symbol.Trim().ToUpper();
            Asset? asset = await _context.Assets.FirstOrDefaultAsync(a => a.Symbol == symbol);

            if (asset == null)
            {
                return NotFound("Asset not found.");
            }

            PortfolioHolding? holding = await _context.PortfolioHoldings
                .FirstOrDefaultAsync(p =>
                    p.userID == userID &&
                    p.assetID == asset.assetID);

            if (holding == null || holding.Quantity < request.Quantity)
            {
                return BadRequest("Not enough asset to sell.");
            }

            decimal? livePrice = await _marketDataService.GetLivePriceAsync(symbol);

            if (livePrice == null)
            {
                return BadRequest("Market price is unavailable.");
            }

            decimal executionPrice = Math.Round(livePrice.Value, 2);
            decimal totalValue = Math.Round( executionPrice * request.Quantity, 2);
            Wallet? wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.userID == userID);

            if (wallet == null)
            {
                return BadRequest("Wallet not found.");
            }

            holding.Quantity -= request.Quantity;
            holding.UpdatedAt = DateTime.UtcNow;
            decimal remainingQuantity = holding.Quantity;

            if (holding.Quantity == 0)
            {
                _context.PortfolioHoldings.Remove(holding);
            }

            wallet.AvailableCash = Math.Round(wallet.AvailableCash + totalValue, 2);
            wallet.UpdatedAt = DateTime.UtcNow;
            asset.CurrentPrice = executionPrice;
            asset.LastUpdatedAt = DateTime.UtcNow;

            TradeLedger trade = new TradeLedger
            {
                userID = userID,
                assetID = asset.assetID,
                TradeType = "SELL",
                Quantity = request.Quantity,
                ExecutionPrice = executionPrice,
                TotalValue = totalValue,
                CreatedAt = DateTime.UtcNow
            };

            _context.TradeLedgers.Add(trade);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Sell completed.",
                symbol = symbol,
                soldQuantity = request.Quantity,
                remainingQuantity = remainingQuantity,
                price = executionPrice,
                totalValue = totalValue,
                availableCash = wallet.AvailableCash
            });
        }
    }
}