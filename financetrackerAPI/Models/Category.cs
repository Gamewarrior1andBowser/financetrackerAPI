namespace financetrackerAPI.Models
{
    public class Category
    {
        public int CategoryID { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public int userID { get; set; }


        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}