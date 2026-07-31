using System.ComponentModel.DataAnnotations;

namespace financetrackerAPI.Models
{
    public class Transaction
    {


        [Key]
        public int transactionsID { get; set; }

        public decimal Amount { get; set; }

        public int userID { get; set; }

        public int categoryID { get; set; }

        public Category category { get; set; }
        public DateTime Date { get; set; }
    }
}
