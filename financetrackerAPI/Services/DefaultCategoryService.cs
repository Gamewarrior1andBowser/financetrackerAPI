using financetrackerAPI.Models;


public class DefaultCategoryService
{
    public static List<Category> CreateDefaultCategories(int userID)
    {
        return new List<Category>
        {
            new Category
            {
                name = "Rent",
                Type = "Expense",
                userID = userID
            },
            new Category
            {
                name = "Food",
                Type = "Expense",
                userID = userID
            },
            new Category
            {
                name = "Work",
                Type = "Income",
                userID = userID
            }
        };
    }
}