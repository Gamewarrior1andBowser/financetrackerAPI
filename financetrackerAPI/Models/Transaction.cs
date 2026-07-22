namespace financetrackerAPI.Models
{
    public class Transaction
    {
        public int TransactionID { get; set; }

        public decimal Amount { get; set; }

        public int UserID { get; set; }

        public int CategoryID { get; set; }
        public DateTime Date { get; set; }
    }
}
