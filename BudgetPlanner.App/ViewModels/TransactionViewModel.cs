using BudgetPlanner.App.Commands;
using BudgetPlanner.App.Helpers;
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
    /// ViewModel za upravljanje transakcijama.
    /// </summary>
    public class TransactionViewModel : ViewModelBase
    {
        private readonly Repository<Transaction> _transactionRepository;
        private readonly Repository<Category> _categoryRepository;
        
        private ObservableCollection<Transaction> _transactions = new();
        private ObservableCollection<Category> _categories = new();
        private ObservableCollection<Category> _allCategories = new();
        private Transaction? _selectedTransaction;
        private string _transactionType = "Expense";
        private decimal _amount;
        private string _description = string.Empty;
        private DateTime _date = DateTime.Today;
        private Category? _selectedCategory;
        private string _searchText = string.Empty;

        // Filter properties
        private DateTime? _filterStartDate;
        private DateTime? _filterEndDate;
        private Category? _filterCategory;
        
        public ObservableCollection<Transaction> Transactions
        {
            get => _transactions;
            set => SetProperty(ref _transactions, value);
        }
        
        public ObservableCollection<Category> Categories
        {
            get => _categories;
            set => SetProperty(ref _categories, value);
        }
        
        public Transaction? SelectedTransaction
        {
            get => _selectedTransaction;
            set
            {
                if (SetProperty(ref _selectedTransaction, value) && value != null)
                {
                    LoadTransactionDetails(value);
                }
            }
        }
        
        public string TransactionType
        {
            get => _transactionType;
            set
            {
                if (SetProperty(ref _transactionType, value))
                {
                    LoadCategories();
                }
            }
        }
        
        public decimal Amount
        {
            get => _amount;
            set => SetProperty(ref _amount, value);
        }
        
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }
        
        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }
        
        public Category? SelectedCategory
        {
            get => _selectedCategory;
            set => SetProperty(ref _selectedCategory, value);
        }
        
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    LoadTransactions();
                }
            }
        }

        public ObservableCollection<Category> AllCategories
        {
            get => _allCategories;
            set => SetProperty(ref _allCategories, value);
        }

        public DateTime? FilterStartDate
        {
            get => _filterStartDate;
            set
            {
                if (SetProperty(ref _filterStartDate, value))
                {
                    LoadTransactions();
                }
            }
        }

        public DateTime? FilterEndDate
        {
            get => _filterEndDate;
            set
            {
                if (SetProperty(ref _filterEndDate, value))
                {
                    LoadTransactions();
                }
            }
        }

        public Category? FilterCategory
        {
            get => _filterCategory;
            set
            {
                if (SetProperty(ref _filterCategory, value))
                {
                    LoadTransactions();
                }
            }
        }

        public ICommand AddTransactionCommand { get; }
        public ICommand UpdateTransactionCommand { get; }
        public ICommand DeleteTransactionCommand { get; }
        public ICommand ClearFormCommand { get; }
        public ICommand ClearFiltersCommand { get; }
        
        public TransactionViewModel()
        {
            _transactionRepository = new Repository<Transaction>();
            _categoryRepository = new Repository<Category>();
            
            AddTransactionCommand = new RelayCommand(async _ => await AddTransaction(), _ => CanAddTransaction());
            UpdateTransactionCommand = new RelayCommand(async _ => await UpdateTransaction(), _ => SelectedTransaction != null);
            DeleteTransactionCommand = new RelayCommand(async _ => await DeleteTransaction(), _ => SelectedTransaction != null);
            ClearFormCommand = new RelayCommand(_ => ClearForm());
            ClearFiltersCommand = new RelayCommand(_ => ClearFilters());

            LoadAllCategories();
            LoadCategories();
            LoadTransactions();
        }
        
        private bool CanAddTransaction()
        {
            return Amount > 0 && 
                   !string.IsNullOrWhiteSpace(Description) && 
                   SelectedCategory != null;
        }
        
        private async System.Threading.Tasks.Task AddTransaction()
        {
            try
            {
                // Validacija
                if (!ValidationHelper.IsPositiveAmount(Amount))
                {
                    MessageBox.Show("Iznos mora biti pozitivan broj!", "Validaciona greška",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!ValidationHelper.IsNotEmpty(Description))
                {
                    MessageBox.Show("Opis ne sme biti prazan!", "Validaciona greška",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var transaction = TransactionFactory.CreateTransaction(
                    TransactionType,
                    Amount,
                    Description,
                    Date,
                    SelectedCategory!.Id,
                    UserSession.Instance.CurrentUser!.Id
                );

                await _transactionRepository.AddAsync(transaction);
                await LoadTransactions();
                ClearForm();

                // Notify MainViewModel to refresh dashboard
                RaiseTransactionChanged();

                MessageBox.Show("Transakcija je uspešno dodata!", "Uspeh",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri dodavanju transakcije: {ex.Message}", "Greška",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private async System.Threading.Tasks.Task UpdateTransaction()
        {
            if (SelectedTransaction == null) return;
            
            try
            {
                SelectedTransaction.Amount = Amount;
                SelectedTransaction.Description = Description;
                SelectedTransaction.Date = Date;
                SelectedTransaction.CategoryId = SelectedCategory!.Id;
                
                await _transactionRepository.UpdateAsync(SelectedTransaction);
                await LoadTransactions();
                ClearForm();

                // Notify MainViewModel to refresh dashboard
                RaiseTransactionChanged();

                MessageBox.Show("Transakcija je uspešno ažurirana!", "Uspeh",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri ažuriranju transakcije: {ex.Message}", "Greška",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private async System.Threading.Tasks.Task DeleteTransaction()
        {
            if (SelectedTransaction == null) return;
            
            var result = MessageBox.Show(
                "Da li ste sigurni da želite da obrišete ovu transakciju?",
                "Potvrda brisanja",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await _transactionRepository.DeleteAsync(SelectedTransaction);
                    await LoadTransactions();
                    ClearForm();

                    // Notify MainViewModel to refresh dashboard
                    RaiseTransactionChanged();

                    MessageBox.Show("Transakcija je uspešno obrisana!", "Uspeh",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Greška pri brisanju transakcije: {ex.Message}", "Greška",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        
        private void ClearForm()
        {
            SelectedTransaction = null;
            Amount = 0;
            Description = string.Empty;
            Date = DateTime.Today;
            SelectedCategory = null;
            TransactionType = "Expense";
        }
        
        private void LoadTransactionDetails(Transaction transaction)
        {
            Amount = transaction.Amount;
            Description = transaction.Description;
            Date = transaction.Date;
            TransactionType = transaction.GetTransactionType();
            SelectedCategory = Categories.FirstOrDefault(c => c.Id == transaction.CategoryId);
        }
        
        private async void LoadAllCategories()
        {
            try
            {
                var userId = UserSession.Instance.CurrentUser!.Id;
                var categories = await _categoryRepository.FindAsync(c => c.UserId == userId);

                AllCategories.Clear();
                foreach (var category in categories)
                {
                    AllCategories.Add(category);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri učitavanju svih kategorija: {ex.Message}", "Greška",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void LoadCategories()
        {
            try
            {
                var userId = UserSession.Instance.CurrentUser!.Id;
                var allCategories = await _categoryRepository.FindAsync(c => c.UserId == userId);

                Categories.Clear();
                foreach (var category in allCategories.Where(c => c.GetCategoryType() == TransactionType))
                {
                    Categories.Add(category);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri učitavanju kategorija: {ex.Message}", "Greška",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearFilters()
        {
            FilterStartDate = null;
            FilterEndDate = null;
            FilterCategory = null;
            SearchText = string.Empty;
        }
        
        private async System.Threading.Tasks.Task LoadTransactions()
        {
            try
            {
                var userId = UserSession.Instance.CurrentUser!.Id;
                var query = await _transactionRepository.FindAsync(t => t.UserId == userId);

                // Filter by search text
                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    query = query.Where(t =>
                        t.Description.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
                }

                // Filter by date range
                if (FilterStartDate.HasValue)
                {
                    query = query.Where(t => t.Date >= FilterStartDate.Value);
                }

                if (FilterEndDate.HasValue)
                {
                    query = query.Where(t => t.Date <= FilterEndDate.Value);
                }

                // Filter by category
                if (FilterCategory != null)
                {
                    query = query.Where(t => t.CategoryId == FilterCategory.Id);
                }

                Transactions.Clear();
                foreach (var transaction in query.OrderByDescending(t => t.Date))
                {
                    Transactions.Add(transaction);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri učitavanju transakcija: {ex.Message}", "Greška",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
