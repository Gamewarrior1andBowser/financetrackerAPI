using System.ComponentModel.DataAnnotations;

namespace financetrackerAPI.Models
{
    public class Transaction
    {
        [Key]
        public int transactionsID { get; set; }

        public int amount { get; set; }
        public int userID { get; set; }

        public int categoryID { get; set; }

        public Category? Category { get; set; }

        public string type { get; set; } = string.Empty;

        public DateTime date { get; set; }
        public string? notes { get; set; }

        
    }
}