namespace financetrackerAPI.Models {
    public class Budget {
        public int budgetID { get; set; }

        public int userID { get; set; }

        public DateTime Date { get; set; }

        public int Limit { get; set; }
    }
}
