namespace financetrackerAPI.DTOs
{
    public class TradeRequest
    {
        public string Symbol { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
    }
}