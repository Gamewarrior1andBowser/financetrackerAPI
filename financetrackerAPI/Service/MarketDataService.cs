using System.Text.Json;
using financetrackerAPI.Data;
using financetrackerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace financetrackerAPI.Services
{
    public class MarketDataService
    {
        private readonly AppDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public MarketDataService( AppDbContext context,IHttpClientFactory httpClientFactory, IConfiguration configuration )
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<List<Asset>> GetAllAssetsAsync()
        {
            await AddMockAssetsIfNeededAsync();
            return await _context.Assets
                .OrderBy(a => a.Symbol)
                .ToListAsync();
        }

        public async Task<decimal?> GetLivePriceAsync(string symbol)
        {
            string? apiKey = _configuration["TwelveData:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                return null;
            }
            string cleanSymbol = symbol.Trim().ToUpper();
            Asset? asset = await _context.Assets.FirstOrDefaultAsync(a => a.Symbol == cleanSymbol);
            string externalSymbol = asset?.ExternalApiID ?? cleanSymbol;
            string encodedSymbol = Uri.EscapeDataString(externalSymbol);
            string url = $"https://api.twelvedata.com/quote?symbol={encodedSymbol}&apikey={apiKey}";
            HttpClient client = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            string json = await response.Content.ReadAsStringAsync();
            using JsonDocument document = JsonDocument.Parse(json);
            JsonElement root = document.RootElement;
            if (!root.TryGetProperty("close", out JsonElement closeElement))
            {
                return null;
            }
            string? priceText = closeElement.GetString();
            if (!decimal.TryParse(
                priceText,
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture,
                out decimal price))
            {
                return null;
            }
            return price;
        }

        public async Task<Asset?> GetAssetBySymbolAsync(string symbol)
        {
            await AddMockAssetsIfNeededAsync();
            string cleanSymbol = symbol.Trim().ToUpper();
            return await _context.Assets
                .FirstOrDefaultAsync(a => a.Symbol == cleanSymbol);
        }

        private async Task AddMockAssetsIfNeededAsync()
        {
            bool hasAssets = await _context.Assets.AnyAsync();
            if (hasAssets)
            {
                return;
            }
            DateTime currentTime = DateTime.UtcNow;
            List<Asset> assets = new List<Asset>
            {
                new Asset
                {
                    Symbol = "AAPL",
                    Name = "Apple Inc.",
                    AssetType = "Stock",
                    CurrentPrice = 200.00m,
                    ExternalApiID = "AAPL",
                    LastUpdatedAt = currentTime
                },
                new Asset
                {
                    Symbol = "MSFT",
                    Name = "Microsoft Corporation",
                    AssetType = "Stock",
                    CurrentPrice = 400.00m,
                    ExternalApiID = "MSFT",
                    LastUpdatedAt = currentTime
                },
                new Asset
                {
                    Symbol = "NVDA",
                    Name = "NVIDIA Corporation",
                    AssetType = "Stock",
                    CurrentPrice = 100.00m,
                    ExternalApiID = "NVDA",
                    LastUpdatedAt = currentTime
                },
                new Asset
                {
                    Symbol = "BTC",
                    Name = "Bitcoin",
                    AssetType = "Crypto",
                    CurrentPrice = 100000.00m,
                    ExternalApiID = "BTC/USD",
                    LastUpdatedAt = currentTime
                }
            };

            _context.Assets.AddRange(assets);
            await _context.SaveChangesAsync();
        }
    }
}