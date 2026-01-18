using System;

namespace BudgetPlanner.App.Models
{
    /// <summary>
    /// Predstavlja prihod korisnika.
    /// Nasleđuje apstraktnu klasu Transaction.
    /// </summary>
    public class Income : Transaction
    {
        public string Source { get; set; } = string.Empty; // Izvor prihoda
        public bool IsTaxable { get; set; } = true; // Da li se oporezuje
        
        public override string GetTransactionType()
        {
            return "Income";
        }
        
        public override string FormatAmount()
        {
            return $"+{Amount:C}";
        }
        
        public override string GetIcon()
        {
            return "📈";
        }
    }
}
