using System;

namespace BudgetPlanner.App.Models
{
    /// <summary>
    /// Predstavlja rashod korisnika.
    /// Nasleđuje apstraktnu klasu Transaction.
    /// </summary>
    public class Expense : Transaction
    {
        public string PaymentMethod { get; set; } = "Cash"; // Keš, kartica, itd.
        public bool IsPlanned { get; set; } = false; // Da li je planiran rashod
        
        public override string GetTransactionType()
        {
            return "Expense";
        }
        
        public override string FormatAmount()
        {
            return $"-{Amount:C}";
        }
        
        public override string GetIcon()
        {
            return "📉";
        }
    }
}
