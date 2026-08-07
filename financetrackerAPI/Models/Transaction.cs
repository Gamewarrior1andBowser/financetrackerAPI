using System.ComponentModel.DataAnnotations;

namespace financetrackerAPI.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionID { get; set; }

        public decimal Amount { get; set; }

        public string Type { get; set; } = string.Empty;

        public int userID { get; set; }

        public int categoryID { get; set; }

        public Category? Category { get; set; }

        public DateTime Date { get; set; }
    }
}