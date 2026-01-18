using BudgetPlanner.App.Models;
using System;

namespace BudgetPlanner.App.Services
{
    /// <summary>
    /// Upravlja trenutnom sesijom korisnika.
    /// Implementira Singleton pattern - Kreacioni dizajn šablon.
    /// Obezbeđuje da postoji samo jedna sesija kroz celu aplikaciju.
    /// </summary>
    public class UserSession
    {
        private static UserSession? _instance;
        private static readonly object _lock = new object();
        
        public User? CurrentUser { get; private set; }
        public bool IsLoggedIn => CurrentUser != null;
        
        public event EventHandler? UserLoggedIn;
        public event EventHandler? UserLoggedOut;
        
        /// <summary>
        /// Singleton instanca UserSession-a.
        /// Thread-safe implementacija.
        /// </summary>
        public static UserSession Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new UserSession();
                        }
                    }
                }
                return _instance;
            }
        }
        
        private UserSession()
        {
        }
        
        /// <summary>
        /// Prijavljuje korisnika u sistem.
        /// </summary>
        public void Login(User user)
        {
            CurrentUser = user;
            UserLoggedIn?.Invoke(this, EventArgs.Empty);
        }
        
        /// <summary>
        /// Odjavljuje trenutnog korisnika.
        /// </summary>
        public void Logout()
        {
            CurrentUser = null;
            UserLoggedOut?.Invoke(this, EventArgs.Empty);
        }
        
        /// <summary>
        /// Resetuje Singleton instancu (korisno za testiranje).
        /// </summary>
        public static void ResetInstance()
        {
            lock (_lock)
            {
                _instance?.Logout();
                _instance = null;
            }
        }
    }
}
