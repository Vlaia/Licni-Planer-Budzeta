using System;

namespace BudgetPlanner.App.Models
{
    /// <summary>
    /// Predstavlja mesečni finansijski izveštaj korisnika.
    /// </summary>
    public class MonthlyReport
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal Balance { get; set; }
        public DateTime GeneratedAt { get; set; } = DateTime.Now;
        
        // Navigation properties
        public virtual User User { get; set; } = null!;
        
        /// <summary>
        /// Vraća period izveštaja.
        /// </summary>
        public string GetPeriod()
        {
            return $"{Year}-{Month:D2}";
        }
        
        /// <summary>
        /// Vraća naziv meseca.
        /// </summary>
        public string GetMonthName()
        {
            return new DateTime(Year, Month, 1).ToString("MMMM yyyy");
        }
        
        /// <summary>
        /// Proverava da li je bilans pozitivan.
        /// </summary>
        public bool IsBalancePositive()
        {
            return Balance >= 0;
        }
    }
}
