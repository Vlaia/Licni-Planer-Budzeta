using System;
using System.Collections.Generic;

namespace BudgetPlanner.App.Models
{
    /// <summary>
    /// Apstraktna bazna klasa za sve kategorije.
    /// Demonstracija nasleđivanja i apstraktnih klasa.
    /// </summary>
    public abstract class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Color { get; set; } = "#808080"; // Hex color za UI
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        
        /// <summary>
        /// Apstraktna metoda koja vraća tip kategorije.
        /// Mora biti implementirana u izvedenim klasama.
        /// </summary>
        public abstract string GetCategoryType();
        
        /// <summary>
        /// Virtuelna metoda za dobijanje ikone kategorije.
        /// Može biti override-ovana u izvedenim klasama.
        /// </summary>
        public virtual string GetIcon()
        {
            return "📁";
        }
    }
}
