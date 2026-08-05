using System.ComponentModel.DataAnnotations;

namespace financetrackerAPI.Models
{
    public class Transaction
    {


        [Key]
        public int transactionID { get; set; }


        public decimal Amount { get; set; }


        public int userID { get; set; }


        public int categoryID { get; set; }


        public Category? Category { get; set; }


        public DateTime Date { get; set; }
    }
}