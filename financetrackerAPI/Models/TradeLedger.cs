using System.ComponentModel.DataAnnotations;

namespace financetrackerAPI.Models
{
    public class TradeLedger
    {
        [Key]
        public int tradeID { get; set; }
        public int userID { get; set; }
        public int assetID { get; set; }
        public string TradeType { get; set; }
        public decimal Quantity { get; set; }
        public decimal ExecutionPrice { get; set; }
        public decimal TotalValue { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}