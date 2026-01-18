using BudgetPlanner.App.Models;
using BudgetPlanner.App.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BudgetPlanner.Tests.Services
{
    /// <summary>
    /// Jedinični testovi za UserSession (Singleton pattern).
    /// </summary>
    [TestClass]
    public class UserSessionTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            // Reset Singleton pre svakog testa
            UserSession.ResetInstance();
        }
        
        [TestMethod]
        public void Instance_MultipleCalls_ReturnsSameInstance()
        {
            // Act
            var instance1 = UserSession.Instance;
            var instance2 = UserSession.Instance;
            
            // Assert
            Assert.AreSame(instance1, instance2);
        }
        
        [TestMethod]
        public void Login_SetsCurrentUser()
        {
            // Arrange
            var session = UserSession.Instance;
            var user = new User { Id = 1, Username = "test" };
            
            // Act
            session.Login(user);
            
            // Assert
            Assert.IsNotNull(session.CurrentUser);
            Assert.AreEqual(user.Id, session.CurrentUser.Id);
            Assert.IsTrue(session.IsLoggedIn);
        }
        
        [TestMethod]
        public void Logout_ClearsCurrentUser()
        {
            // Arrange
            var session = UserSession.Instance;
            var user = new User { Id = 1, Username = "test" };
            session.Login(user);
            
            // Act
            session.Logout();
            
            // Assert
            Assert.IsNull(session.CurrentUser);
            Assert.IsFalse(session.IsLoggedIn);
        }
        
        [TestMethod]
        public void Login_RaisesLoginEvent()
        {
            // Arrange
            var session = UserSession.Instance;
            var user = new User { Id = 1, Username = "test" };
            bool eventRaised = false;
            session.UserLoggedIn += (s, e) => eventRaised = true;
            
            // Act
            session.Login(user);
            
            // Assert
            Assert.IsTrue(eventRaised);
        }
        
        [TestMethod]
        public void Logout_RaisesLogoutEvent()
        {
            // Arrange
            var session = UserSession.Instance;
            var user = new User { Id = 1, Username = "test" };
            session.Login(user);
            bool eventRaised = false;
            session.UserLoggedOut += (s, e) => eventRaised = true;
            
            // Act
            session.Logout();
            
            // Assert
            Assert.IsTrue(eventRaised);
        }
    }
}
