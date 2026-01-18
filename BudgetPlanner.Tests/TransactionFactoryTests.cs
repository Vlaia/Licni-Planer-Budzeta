using BudgetPlanner.App.Models;
using BudgetPlanner.App.Services;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BudgetPlanner.Tests.Services
{
    /// <summary>
    /// Jedinični testovi za TransactionFactory (Factory Method pattern).
    /// </summary>
    [TestClass]
    public class TransactionFactoryTests
    {
        [TestMethod]
        public void CreateTransaction_Income_ReturnsIncomeInstance()
        {
            // Arrange
            var type = "income";
            var amount = 1000m;
            var description = "Plata";
            var date = DateTime.Today;
            var categoryId = 1;
            var userId = 1;
            
            // Act
            var transaction = TransactionFactory.CreateTransaction(
                type, amount, description, date, categoryId, userId);
            
            // Assert
            Assert.IsNotNull(transaction);
            Assert.IsInstanceOfType(transaction, typeof(Income));
            Assert.AreEqual(amount, transaction.Amount);
            Assert.AreEqual(description, transaction.Description);
            Assert.AreEqual("Income", transaction.GetTransactionType());
        }
        
        [TestMethod]
        public void CreateTransaction_Expense_ReturnsExpenseInstance()
        {
            // Arrange
            var type = "expense";
            var amount = 50m;
            var description = "Hrana";
            var date = DateTime.Today;
            var categoryId = 2;
            var userId = 1;
            
            // Act
            var transaction = TransactionFactory.CreateTransaction(
                type, amount, description, date, categoryId, userId);
            
            // Assert
            Assert.IsNotNull(transaction);
            Assert.IsInstanceOfType(transaction, typeof(Expense));
            Assert.AreEqual(amount, transaction.Amount);
            Assert.AreEqual("Expense", transaction.GetTransactionType());
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateTransaction_InvalidType_ThrowsException()
        {
            // Arrange
            var type = "invalid";
            
            // Act
            TransactionFactory.CreateTransaction(type, 100, "Test", DateTime.Today, 1, 1);
            
            // Assert is handled by ExpectedException attribute
        }
        
        [TestMethod]
        public void CreateIncome_WithParameters_ReturnsConfiguredIncome()
        {
            // Arrange
            var amount = 2000m;
            var source = "Freelance";
            
            // Act
            var income = TransactionFactory.CreateIncome(
                amount, "Projekat", DateTime.Today, 1, 1, source, true);
            
            // Assert
            Assert.AreEqual(source, income.Source);
            Assert.IsTrue(income.IsTaxable);
            Assert.AreEqual(amount, income.Amount);
        }
        
        [TestMethod]
        public void CreateExpense_WithParameters_ReturnsConfiguredExpense()
        {
            // Arrange
            var amount = 100m;
            var paymentMethod = "Card";
            
            // Act
            var expense = TransactionFactory.CreateExpense(
                amount, "Kupovina", DateTime.Today, 2, 1, paymentMethod, true);
            
            // Assert
            Assert.AreEqual(paymentMethod, expense.PaymentMethod);
            Assert.IsTrue(expense.IsPlanned);
            Assert.AreEqual(amount, expense.Amount);
        }
    }
}
