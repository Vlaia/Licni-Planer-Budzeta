using System;

namespace BudgetPlanner.App.Models
{
    /// <summary>
    /// Apstraktna bazna klasa za sve transakcije.
    /// Demonstracija nasleđivanja i polimorfizma.
    /// </summary>
    public abstract class Transaction
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Today;
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // Navigation properties
        public virtual Category Category { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        
        /// <summary>
        /// Apstraktna metoda koja vraća tip transakcije.
        /// </summary>
        public abstract string GetTransactionType();
        
        /// <summary>
        /// Apstraktna metoda za formatiranje prikaza iznosa.
        /// Prihodi sa +, rashodi sa -.
        /// </summary>
        public abstract string FormatAmount();
        
        /// <summary>
        /// Virtuelna metoda za dobijanje ikone transakcije.
        /// </summary>
        public virtual string GetIcon()
        {
            return "💱";
        }
    }
}
