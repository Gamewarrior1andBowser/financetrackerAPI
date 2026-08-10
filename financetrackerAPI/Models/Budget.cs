using System.ComponentModel.DataAnnotations.Schema;

namespace financetrackerAPI.Models
{
    [Table("Budget")]
    public class Budget
    {
        public int budgetID { get; set; }

        public int userID { get; set; }

        public string username { get; set; } = string.Empty;

        public decimal limits { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;
    }
}