using BudgetPlanner.App.Models;
using BudgetPlanner.App.Services;
using BudgetPlanner.App.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BudgetPlanner.Tests.ViewModels
{
    /// <summary>
    /// Jedinični testovi za TransactionViewModel.
    /// </summary>
    [TestClass]
    public class TransactionViewModelTests
    {
        [TestMethod]
        public void Constructor_InitializesCollections()
        {
            // Arrange & Act
            var viewModel = new TransactionViewModel();
            
            // Assert
            Assert.IsNotNull(viewModel.Transactions);
            Assert.IsNotNull(viewModel.Categories);
            Assert.IsNotNull(viewModel.AddTransactionCommand);
            Assert.IsNotNull(viewModel.UpdateTransactionCommand);
            Assert.IsNotNull(viewModel.DeleteTransactionCommand);
        }
        
        [TestMethod]
        public void Amount_SetValue_UpdatesProperty()
        {
            // Arrange
            var viewModel = new TransactionViewModel();
            var amount = 100.50m;
            
            // Act
            viewModel.Amount = amount;
            
            // Assert
            Assert.AreEqual(amount, viewModel.Amount);
        }
        
        [TestMethod]
        public void Description_SetValue_UpdatesProperty()
        {
            // Arrange
            var viewModel = new TransactionViewModel();
            var description = "Test transakcija";
            
            // Act
            viewModel.Description = description;
            
            // Assert
            Assert.AreEqual(description, viewModel.Description);
        }
    }
}
