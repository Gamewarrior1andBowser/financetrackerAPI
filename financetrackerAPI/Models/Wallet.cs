using System.ComponentModel.DataAnnotations;

namespace financetrackerAPI.Models
{
    public class Wallet
    {
        [Key]
        public int walletID { get; set; }
        public int userID { get; set; }
        public decimal AvailableCash { get; set; }
        public decimal InitialCash { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}