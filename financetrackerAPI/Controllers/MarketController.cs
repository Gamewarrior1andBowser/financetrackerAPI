using Microsoft.AspNetCore.Authentication.JwtBearer;
using financetrackerAPI.Models;
using financetrackerAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace financetrackerAPI.Controllers
{
    [Route("api/market")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MarketController : ControllerBase
    {
        private readonly MarketDataService _marketDataService;
        public MarketController(MarketDataService marketDataService)
        {
            _marketDataService = marketDataService;
        }

        [HttpGet("assets")]
        public async Task<IActionResult> GetAssets()
        {
            List<Asset> assets = await _marketDataService.GetAllAssetsAsync();
            return Ok(assets.Select(a => new
            {
                assetID = a.assetID,
                symbol = a.Symbol,
                name = a.Name,
                assetType = a.AssetType,
                currentPrice = a.CurrentPrice,
                lastUpdatedAt = a.LastUpdatedAt,
                source = "Mock"
            }));
        }

        [HttpGet("assets/{symbol}")]
        public async Task<IActionResult> GetAsset(string symbol)
        {
            Asset? asset = await _marketDataService.GetAssetBySymbolAsync(symbol);
            if (asset == null)
            {
                return NotFound("Asset not found.");
            }
            return Ok(new
            {
                assetID = asset.assetID,
                symbol = asset.Symbol,
                name = asset.Name,
                assetType = asset.AssetType,
                currentPrice = asset.CurrentPrice,
                lastUpdatedAt = asset.LastUpdatedAt,
                source = "Mock"
            });
        }

        [HttpGet("live/{symbol}")]
        public async Task<IActionResult> GetLivePrice(string symbol)
        {
            decimal? price = await _marketDataService.GetLivePriceAsync(symbol);
            if (price == null)
            {
                return StatusCode(
                    503,
                    "Live market price is currently unavailable."
                );
            }
            return Ok(new
            {
                symbol = symbol.Trim().ToUpper(),
                currentPrice = price,
                source = "TwelveData",
                updatedAt = DateTime.UtcNow
            });
        }
    }
}