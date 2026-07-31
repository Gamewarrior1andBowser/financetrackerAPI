using Microsoft.EntityFrameworkCore.Query.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace financetrackerAPI.Models {

    [Table("Budget")]
    public class Budget {
        public int budgetID { get; set; }

        public int userID { get; set; }

        public DateTime Date { get; set; }

        public Category category { get; set; }

        public int categoryID { get; set; }



        [Column("limits")]
        public int Limit { get; set; }
    }
}
