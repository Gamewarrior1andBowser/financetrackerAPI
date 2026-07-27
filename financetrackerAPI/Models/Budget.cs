namespace financetrackerAPI.Models {
    public class Budget {
        public int BudgetID { get; set; }

        public int UserID { get; set; }

        public DateTime Date { get; set; }

        public int Limit { get; set; }
    }
}
