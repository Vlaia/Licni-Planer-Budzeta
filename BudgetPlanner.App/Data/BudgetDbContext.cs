using Microsoft.EntityFrameworkCore;
using BudgetPlanner.App.Models;
using System;
using System.IO;

namespace BudgetPlanner.App.Data
{
    /// <summary>
    /// Database context za aplikaciju.
    /// Implementira Singleton pattern - Kreacioni dizajn šablon.
    /// Obezbeđuje da postoji samo jedna instanca DbContext-a kroz celu aplikaciju.
    /// </summary>
    public class BudgetDbContext : DbContext
    {
        private static BudgetDbContext? _instance;
        private static readonly object _lock = new object();
        
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<IncomeCategory> IncomeCategories { get; set; } = null!;
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; } = null!;
        public DbSet<Transaction> Transactions { get; set; } = null!;
        public DbSet<Income> Incomes { get; set; } = null!;
        public DbSet<Expense> Expenses { get; set; } = null!;
        public DbSet<Budget> Budgets { get; set; } = null!;
        public DbSet<MonthlyReport> MonthlyReports { get; set; } = null!;
        
        /// <summary>
        /// Singleton instanca DbContext-a.
        /// Thread-safe implementacija koristeći double-check locking pattern.
        /// </summary>
        public static BudgetDbContext Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new BudgetDbContext();
                            _instance.Database.EnsureCreated();
                        }
                    }
                }
                return _instance;
            }
        }
        
        private BudgetDbContext()
        {
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string dbPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "BudgetPlanner",
                    "budget.db"
                );
                
                Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);
                
                optionsBuilder.UseSqlite($"Data Source={dbPath}");
            }
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Konfiguracija nasleđivanja za Category
            modelBuilder.Entity<Category>()
                .HasDiscriminator<string>("CategoryType")
                .HasValue<IncomeCategory>("Income")
                .HasValue<ExpenseCategory>("Expense");
            
            // Konfiguracija nasleđivanja za Transaction
            modelBuilder.Entity<Transaction>()
                .HasDiscriminator<string>("TransactionType")
                .HasValue<Income>("Income")
                .HasValue<Expense>("Expense");
            
            // Decimal precision
            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasPrecision(18, 2);
            
            modelBuilder.Entity<Budget>()
                .Property(b => b.PlannedAmount)
                .HasPrecision(18, 2);
            
            modelBuilder.Entity<ExpenseCategory>()
                .Property(e => e.MaxMonthlyBudget)
                .HasPrecision(18, 2);
            
            modelBuilder.Entity<MonthlyReport>()
                .Property(m => m.TotalIncome)
                .HasPrecision(18, 2);
            
            modelBuilder.Entity<MonthlyReport>()
                .Property(m => m.TotalExpenses)
                .HasPrecision(18, 2);
            
            modelBuilder.Entity<MonthlyReport>()
                .Property(m => m.Balance)
                .HasPrecision(18, 2);
            
            // Relacije
            modelBuilder.Entity<Category>()
                .HasOne(c => c.User)
                .WithMany(u => u.Categories)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Budget>()
                .HasOne(b => b.User)
                .WithMany(u => u.Budgets)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Budget>()
                .HasOne(b => b.Category)
                .WithMany()
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Seed data - Default korisnik
            // BCrypt hash od "admin123"
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    PasswordHash = "$2a$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LewY5GyYVvhOiwZFu", // BCrypt hash od "admin123"
                    Email = "admin@budgetplanner.com",
                    FullName = "Administrator",
                    CreatedAt = new DateTime(2024, 1, 1)
                }
            );
        }
        
        /// <summary>
        /// Resetuje Singleton instancu (korisno za testiranje).
        /// </summary>
        public static void ResetInstance()
        {
            lock (_lock)
            {
                _instance?.Dispose();
                _instance = null;
            }
        }
    }
}
