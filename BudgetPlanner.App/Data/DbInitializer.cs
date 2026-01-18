using BudgetPlanner.App.Models;
using System;
using System.Linq;
using System.Windows;

namespace BudgetPlanner.App.Data
{
    /// <summary>
    /// Inicijalizator baze podataka.
    /// Dodaje početne podatke ako ne postoje.
    /// </summary>
    public static class DbInitializer
    {
        public static void Initialize(BudgetDbContext context)
        {
            try
            {
                context.Database.EnsureCreated();

                // Proveri koliko kategorija već postoji
                var categoryCount = context.Categories.Count();

                if (categoryCount >= 10)
                {
                    // Već ima dovoljno kategorija
                    return;
                }

                if (categoryCount > 0)
                {
                    var result = MessageBox.Show(
                        $"Baza već sadrži {categoryCount} kategorija.\n\n" +
                        "Da li želite da OBRIŠETE postojeće kategorije i dodate svih 10 početnih kategorija?\n\n" +
                        "Kliknite YES da obrišete postojeće i dodate nove\n" +
                        "Kliknite NO da zadržite postojeće",
                        "Kategorije",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            // Prvo obriši sve transakcije koje koriste ove kategorije
                            var existingTransactions = context.Transactions.ToList();
                            if (existingTransactions.Any())
                            {
                                context.Transactions.RemoveRange(existingTransactions);
                                context.SaveChanges();
                            }

                            // Obriši sve budžete
                            var existingBudgets = context.Budgets.ToList();
                            if (existingBudgets.Any())
                            {
                                context.Budgets.RemoveRange(existingBudgets);
                                context.SaveChanges();
                            }

                            // Sada obriši kategorije
                            var existingCategories = context.Categories.ToList();
                            context.Categories.RemoveRange(existingCategories);
                            context.SaveChanges();

                            MessageBox.Show("Postojeće kategorije obrisane. Dodajem novih 10...",
                                "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        catch (Exception deleteEx)
                        {
                            MessageBox.Show($"Greška pri brisanju postojećih podataka: {deleteEx.Message}\n\n{deleteEx.InnerException?.Message}",
                                "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri proveri baze: {ex.Message}",
                    "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Dodaj kategorije rashoda
            var expenseCategories = new ExpenseCategory[]
            {
                new ExpenseCategory
                {
                    Name = "Hrana",
                    Description = "Kupovina hrane i namirnica",
                    Color = "#FF6B6B",
                    UserId = 1,
                    IsEssential = true,
                    MaxMonthlyBudget = 0,
                    CreatedAt = DateTime.Now
                },
                new ExpenseCategory
                {
                    Name = "Transport",
                    Description = "Gorivo, prevoz, parking",
                    Color = "#4ECDC4",
                    UserId = 1,
                    IsEssential = true,
                    MaxMonthlyBudget = 0,
                    CreatedAt = DateTime.Now
                },
                new ExpenseCategory
                {
                    Name = "Računi",
                    Description = "Komunalije, internet, telefon",
                    Color = "#FFE66D",
                    UserId = 1,
                    IsEssential = true,
                    MaxMonthlyBudget = 0,
                    CreatedAt = DateTime.Now
                },
                new ExpenseCategory
                {
                    Name = "Zabava",
                    Description = "Izlasci, bioskop, restorani",
                    Color = "#95E1D3",
                    UserId = 1,
                    IsEssential = false,
                    MaxMonthlyBudget = 0,
                    CreatedAt = DateTime.Now
                },
                new ExpenseCategory
                {
                    Name = "Odeca",
                    Description = "Kupovina garderobe i obuće",
                    Color = "#F38181",
                    UserId = 1,
                    IsEssential = false,
                    MaxMonthlyBudget = 0,
                    CreatedAt = DateTime.Now
                },
                new ExpenseCategory
                {
                    Name = "Zdravlje",
                    Description = "Lekovi, lekarske usluge",
                    Color = "#AA96DA",
                    UserId = 1,
                    IsEssential = true,
                    MaxMonthlyBudget = 0,
                    CreatedAt = DateTime.Now
                }
            };

            context.ExpenseCategories.AddRange(expenseCategories);

            // Dodaj kategorije prihoda
            var incomeCategories = new IncomeCategory[]
            {
                new IncomeCategory
                {
                    Name = "Plata",
                    Description = "Mesečna plata",
                    Color = "#51CF66",
                    UserId = 1,
                    IsRecurring = true,
                    CreatedAt = DateTime.Now
                },
                new IncomeCategory
                {
                    Name = "Honorar",
                    Description = "Freelance i dodatni poslovi",
                    Color = "#74C0FC",
                    UserId = 1,
                    IsRecurring = false,
                    CreatedAt = DateTime.Now
                },
                new IncomeCategory
                {
                    Name = "Poklon",
                    Description = "Novčani pokloni",
                    Color = "#FFD43B",
                    UserId = 1,
                    IsRecurring = false,
                    CreatedAt = DateTime.Now
                },
                new IncomeCategory
                {
                    Name = "Investicije",
                    Description = "Prihod od investicija",
                    Color = "#63E6BE",
                    UserId = 1,
                    IsRecurring = false,
                    CreatedAt = DateTime.Now
                }
            };

            context.IncomeCategories.AddRange(incomeCategories);

            try
            {
                context.SaveChanges();
                MessageBox.Show($"Uspešno dodato {expenseCategories.Length + incomeCategories.Length} kategorija!",
                    "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri čuvanju kategorija: {ex.Message}",
                    "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
