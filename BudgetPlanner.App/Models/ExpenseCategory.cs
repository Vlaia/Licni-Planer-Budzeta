namespace BudgetPlanner.App.Models
{
    /// <summary>
    /// Kategorija rashoda.
    /// Nasleđuje apstraktnu klasu Category.
    /// </summary>
    public class ExpenseCategory : Category
    {
        public bool IsEssential { get; set; } // Da li je neophodan rashod
        public decimal MaxMonthlyBudget { get; set; } // Maksimalan budžet za ovu kategoriju
        
        public override string GetCategoryType()
        {
            return "Expense";
        }
        
        public override string GetIcon()
        {
            return "💸";
        }
    }
}
