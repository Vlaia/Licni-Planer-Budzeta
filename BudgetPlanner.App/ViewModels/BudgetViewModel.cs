using BudgetPlanner.App.Commands;
using BudgetPlanner.App.Models;
using BudgetPlanner.App.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BudgetPlanner.App.ViewModels
{
    /// <summary>
    /// ViewModel za upravljanje budžetima.
    /// </summary>
    public class BudgetViewModel : ViewModelBase
    {
        private readonly Repository<Budget> _budgetRepository;
        private readonly Repository<Category> _categoryRepository;
        
        private ObservableCollection<Budget> _budgets = new();
        private ObservableCollection<Category> _expenseCategories = new();
        private Budget? _selectedBudget;
        private Category? _selectedCategory;
        private decimal _plannedAmount;
        private int _month = DateTime.Now.Month;
        private int _year = DateTime.Now.Year;
        
        public ObservableCollection<Budget> Budgets
        {
            get => _budgets;
            set => SetProperty(ref _budgets, value);
        }
        
        public ObservableCollection<Category> ExpenseCategories
        {
            get => _expenseCategories;
            set => SetProperty(ref _expenseCategories, value);
        }
        
        public Budget? SelectedBudget
        {
            get => _selectedBudget;
            set
            {
                if (SetProperty(ref _selectedBudget, value) && value != null)
                {
                    LoadBudgetDetails(value);
                }
            }
        }
        
        public Category? SelectedCategory
        {
            get => _selectedCategory;
            set => SetProperty(ref _selectedCategory, value);
        }
        
        public decimal PlannedAmount
        {
            get => _plannedAmount;
            set => SetProperty(ref _plannedAmount, value);
        }
        
        public int Month
        {
            get => _month;
            set => SetProperty(ref _month, value);
        }
        
        public int Year
        {
            get => _year;
            set => SetProperty(ref _year, value);
        }
        
        public ICommand AddBudgetCommand { get; }
        public ICommand UpdateBudgetCommand { get; }
        public ICommand DeleteBudgetCommand { get; }
        public ICommand ClearFormCommand { get; }
        
        public BudgetViewModel()
        {
            _budgetRepository = new Repository<Budget>();
            _categoryRepository = new Repository<Category>();
            
            AddBudgetCommand = new RelayCommand(async _ => await AddBudget(), _ => CanAddBudget());
            UpdateBudgetCommand = new RelayCommand(async _ => await UpdateBudget(), _ => SelectedBudget != null);
            DeleteBudgetCommand = new RelayCommand(async _ => await DeleteBudget(), _ => SelectedBudget != null);
            ClearFormCommand = new RelayCommand(_ => ClearForm());
            
            LoadExpenseCategories();
            LoadBudgets();
        }
        
        private bool CanAddBudget()
        {
            return PlannedAmount > 0 && SelectedCategory != null;
        }
        
        private async System.Threading.Tasks.Task AddBudget()
        {
            try
            {
                var budget = new Budget
                {
                    CategoryId = SelectedCategory!.Id,
                    UserId = UserSession.Instance.CurrentUser!.Id,
                    PlannedAmount = PlannedAmount,
                    Month = Month,
                    Year = Year
                };
                
                await _budgetRepository.AddAsync(budget);
                await LoadBudgets();
                ClearForm();
                
                MessageBox.Show("Budžet je uspešno dodat!", "Uspeh",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri dodavanju budžeta: {ex.Message}", "Greška",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private async System.Threading.Tasks.Task UpdateBudget()
        {
            if (SelectedBudget == null) return;
            
            try
            {
                SelectedBudget.PlannedAmount = PlannedAmount;
                SelectedBudget.CategoryId = SelectedCategory!.Id;
                SelectedBudget.Month = Month;
                SelectedBudget.Year = Year;
                
                await _budgetRepository.UpdateAsync(SelectedBudget);
                await LoadBudgets();
                ClearForm();
                
                MessageBox.Show("Budžet je uspešno ažuriran!", "Uspeh",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri ažuriranju budžeta: {ex.Message}", "Greška",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private async System.Threading.Tasks.Task DeleteBudget()
        {
            if (SelectedBudget == null) return;
            
            var result = MessageBox.Show(
                "Da li ste sigurni da želite da obrišete ovaj budžet?",
                "Potvrda brisanja",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await _budgetRepository.DeleteAsync(SelectedBudget);
                    await LoadBudgets();
                    ClearForm();
                    
                    MessageBox.Show("Budžet je uspešno obrisan!", "Uspeh",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Greška pri brisanju budžeta: {ex.Message}", "Greška",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        
        private void ClearForm()
        {
            SelectedBudget = null;
            SelectedCategory = null;
            PlannedAmount = 0;
            Month = DateTime.Now.Month;
            Year = DateTime.Now.Year;
        }
        
        private void LoadBudgetDetails(Budget budget)
        {
            PlannedAmount = budget.PlannedAmount;
            Month = budget.Month;
            Year = budget.Year;
            SelectedCategory = ExpenseCategories.FirstOrDefault(c => c.Id == budget.CategoryId);
        }
        
        private async void LoadExpenseCategories()
        {
            try
            {
                var userId = UserSession.Instance.CurrentUser!.Id;
                var categories = await _categoryRepository.FindAsync(c => 
                    c.UserId == userId && c is ExpenseCategory);
                
                ExpenseCategories.Clear();
                foreach (var category in categories)
                {
                    ExpenseCategories.Add(category);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri učitavanju kategorija: {ex.Message}", "Greška",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private async System.Threading.Tasks.Task LoadBudgets()
        {
            try
            {
                var userId = UserSession.Instance.CurrentUser!.Id;
                var budgets = await _budgetRepository.FindAsync(b => b.UserId == userId);
                
                Budgets.Clear();
                foreach (var budget in budgets.OrderByDescending(b => b.Year).ThenByDescending(b => b.Month))
                {
                    Budgets.Add(budget);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri učitavanju budžeta: {ex.Message}", "Greška",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
