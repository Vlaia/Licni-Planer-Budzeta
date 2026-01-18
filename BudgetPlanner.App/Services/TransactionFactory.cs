using BudgetPlanner.App.Models;
using System;

namespace BudgetPlanner.App.Services
{
    /// <summary>
    /// Factory klasa za kreiranje transakcija.
    /// Implementira Factory Method pattern - Kreacioni dizajn šablon.
    /// Enkapsulira logiku kreiranja različitih tipova transakcija.
    /// </summary>
    public class TransactionFactory
    {
        /// <summary>
        /// Kreira novu transakciju na osnovu tipa.
        /// </summary>
        public static Transaction CreateTransaction(
            string transactionType,
            decimal amount,
            string description,
            DateTime date,
            int categoryId,
            int userId)
        {
            Transaction transaction;
            
            switch (transactionType.ToLower())
            {
                case "income":
                    transaction = new Income
                    {
                        Amount = amount,
                        Description = description,
                        Date = date,
                        CategoryId = categoryId,
                        UserId = userId,
                        Source = string.Empty,
                        IsTaxable = true
                    };
                    break;
                    
                case "expense":
                    transaction = new Expense
                    {
                        Amount = amount,
                        Description = description,
                        Date = date,
                        CategoryId = categoryId,
                        UserId = userId,
                        PaymentMethod = "Cash",
                        IsPlanned = false
                    };
                    break;
                    
                default:
                    throw new ArgumentException($"Nepoznat tip transakcije: {transactionType}");
            }
            
            return transaction;
        }
        
        /// <summary>
        /// Kreira prihod sa dodatnim parametrima.
        /// </summary>
        public static Income CreateIncome(
            decimal amount,
            string description,
            DateTime date,
            int categoryId,
            int userId,
            string source,
            bool isTaxable = true)
        {
            return new Income
            {
                Amount = amount,
                Description = description,
                Date = date,
                CategoryId = categoryId,
                UserId = userId,
                Source = source,
                IsTaxable = isTaxable
            };
        }
        
        /// <summary>
        /// Kreira rashod sa dodatnim parametrima.
        /// </summary>
        public static Expense CreateExpense(
            decimal amount,
            string description,
            DateTime date,
            int categoryId,
            int userId,
            string paymentMethod,
            bool isPlanned = false)
        {
            return new Expense
            {
                Amount = amount,
                Description = description,
                Date = date,
                CategoryId = categoryId,
                UserId = userId,
                PaymentMethod = paymentMethod,
                IsPlanned = isPlanned
            };
        }
    }
}
