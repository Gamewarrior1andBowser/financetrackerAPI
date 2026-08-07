using System.ComponentModel.DataAnnotations.Schema;

namespace financetrackerAPI.Models
{
    [Table("Budget")]
    public class Budget
    {
        public int budgetID { get; set; }

        public int userID { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Limit { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;


        public int categoryID { get; set; }

        public Category? Category { get; set; }
    }
}