using System.ComponentModel.DataAnnotations;

namespace financetrackerAPI.Models
{
    public class PortfolioHolding
    {
        [Key]
        public int portfolioHoldingID { get; set; }
        public int userID { get; set; }
        public int assetID { get; set; }
        public decimal Quantity { get; set; }
        public decimal AveragePurchasePrice { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}