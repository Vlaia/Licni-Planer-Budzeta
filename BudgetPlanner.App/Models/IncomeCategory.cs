namespace BudgetPlanner.App.Models
{
    /// <summary>
    /// Kategorija prihoda.
    /// Nasleđuje apstraktnu klasu Category.
    /// </summary>
    public class IncomeCategory : Category
    {
        public bool IsRecurring { get; set; } // Da li je redovan prihod (plata)
        
        public override string GetCategoryType()
        {
            return "Income";
        }
        
        public override string GetIcon()
        {
            return "💰";
        }
    }
}
