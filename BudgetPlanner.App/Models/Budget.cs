using System;

namespace BudgetPlanner.App.Models
{
    /// <summary>
    /// Predstavlja budžet za određenu kategoriju u određenom mesecu.
    /// </summary>
    public class Budget
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public decimal PlannedAmount { get; set; }
        public int Month { get; set; } // 1-12
        public int Year { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // Navigation properties
        public virtual Category Category { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        
        /// <summary>
        /// Vraća string reprezentaciju perioda budžeta.
        /// </summary>
        public string GetPeriod()
        {
            return $"{Year}-{Month:D2}";
        }
        
        /// <summary>
        /// Proverava da li je budžet aktivan u datom mesecu.
        /// </summary>
        public bool IsActive(DateTime date)
        {
            return date.Year == Year && date.Month == Month;
        }
    }
}
