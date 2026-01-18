using BudgetPlanner.App.Commands;
using BudgetPlanner.App.Models;
using BudgetPlanner.App.Services;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BudgetPlanner.App.ViewModels
{
    /// <summary>
    /// Glavni ViewModel aplikacije.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly Repository<Transaction> _transactionRepository;
        private readonly Repository<Category> _categoryRepository;
        private readonly Repository<Budget> _budgetRepository;
        private readonly Repository<MonthlyReport> _reportRepository;
        private readonly ExportService _exportService;
        private readonly ReportService _reportService;
        
        private ViewModelBase? _currentViewModel;
        private decimal _totalIncome;
        private decimal _totalExpenses;
        private decimal _balance;
        
        public ViewModelBase? CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }
        
        public User CurrentUser => UserSession.Instance.CurrentUser!;
        
        public decimal TotalIncome
        {
            get => _totalIncome;
            set => SetProperty(ref _totalIncome, value);
        }
        
        public decimal TotalExpenses
        {
            get => _totalExpenses;
            set => SetProperty(ref _totalExpenses, value);
        }
        
        public decimal Balance
        {
            get => _balance;
            set => SetProperty(ref _balance, value);
        }
        
        public ICommand ShowTransactionsCommand { get; }
        public ICommand ShowCategoriesCommand { get; }
        public ICommand ShowBudgetsCommand { get; }
        public ICommand ShowReportsCommand { get; }
        public ICommand ExportDataCommand { get; }
        public ICommand ImportDataCommand { get; }
        public ICommand GenerateReportCommand { get; }
        public ICommand LogoutCommand { get; }
        
        public event Action? LogoutRequested;
        
        public MainViewModel()
        {
            _transactionRepository = new Repository<Transaction>();
            _categoryRepository = new Repository<Category>();
            _budgetRepository = new Repository<Budget>();
            _reportRepository = new Repository<MonthlyReport>();
            _exportService = new ExportService();
            _reportService = new ReportService();
            
            ShowTransactionsCommand = new RelayCommand(_ => ShowTransactions());
            ShowCategoriesCommand = new RelayCommand(_ => ShowCategories());
            ShowBudgetsCommand = new RelayCommand(_ => ShowBudgets());
            ShowReportsCommand = new RelayCommand(_ => ShowReports());
            ExportDataCommand = new RelayCommand(async _ => await ExportData());
            ImportDataCommand = new RelayCommand(async _ => await ImportData());
            GenerateReportCommand = new RelayCommand(async _ => await GenerateReport());
            LogoutCommand = new RelayCommand(_ => Logout());

            // Subscribe na TransactionChanged event
            ViewModelBase.TransactionChanged += LoadDashboardData;

            LoadDashboardData();
            ShowTransactions();
        }
        
        public async void LoadDashboardData()
        {
            try
            {
                var currentMonth = DateTime.Now.Month;
                var currentYear = DateTime.Now.Year;

                var transactions = await _transactionRepository.FindAsync(t =>
                    t.UserId == CurrentUser.Id &&
                    t.Date.Month == currentMonth &&
                    t.Date.Year == currentYear);

                TotalIncome = transactions.OfType<Income>().Sum(i => i.Amount);
                TotalExpenses = transactions.OfType<Expense>().Sum(e => e.Amount);
                Balance = TotalIncome - TotalExpenses;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri učitavanju podataka: {ex.Message}", "Greška",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void ShowTransactions()
        {
            CurrentViewModel = new TransactionViewModel();
        }
        
        private void ShowCategories()
        {
            CurrentViewModel = new CategoryViewModel();
        }
        
        private void ShowBudgets()
        {
            CurrentViewModel = new BudgetViewModel();
        }
        
        private void ShowReports()
        {
            MessageBox.Show("Prikaz izveštaja - funkcionalnost u razvoju", "Izveštaji",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
        
        private async System.Threading.Tasks.Task ExportData()
        {
            try
            {
                var dialog = new SaveFileDialog
                {
                    Filter = _exportService.GetExportFileFilter(),
                    DefaultExt = ".json",
                    FileName = $"BudgetExport_{DateTime.Now:yyyyMMdd}"
                };
                
                if (dialog.ShowDialog() == true)
                {
                    var transactions = await _transactionRepository.FindAsync(t => t.UserId == CurrentUser.Id);
                    
                    if (dialog.FileName.EndsWith(".json"))
                    {
                        _exportService.ExportTransactionsToJson(transactions, dialog.FileName);
                    }
                    else if (dialog.FileName.EndsWith(".xml"))
                    {
                        _exportService.ExportTransactionsToXml(transactions, dialog.FileName);
                    }
                    
                    MessageBox.Show("Podaci su uspešno exportovani!", "Export",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri exportu: {ex.Message}", "Greška",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private async System.Threading.Tasks.Task ImportData()
        {
            try
            {
                var dialog = new OpenFileDialog
                {
                    Filter = _exportService.GetImportFileFilter()
                };
                
                if (dialog.ShowDialog() == true)
                {
                    var result = MessageBox.Show(
                        "Import podataka može dodati nove transakcije. Da li želite da nastavite?",
                        "Potvrda importa",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);
                    
                    if (result == MessageBoxResult.Yes)
                    {
                        // Ovde bi bila logika za import
                        MessageBox.Show("Import funkcionalnost - u razvoju", "Import",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri importu: {ex.Message}", "Greška",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private async System.Threading.Tasks.Task GenerateReport()
        {
            try
            {
                var currentMonth = DateTime.Now.Month;
                var currentYear = DateTime.Now.Year;
                
                var transactions = await _transactionRepository.FindAsync(t =>
                    t.UserId == CurrentUser.Id &&
                    t.Date.Month == currentMonth &&
                    t.Date.Year == currentYear);
                
                var report = new MonthlyReport
                {
                    UserId = CurrentUser.Id,
                    Month = currentMonth,
                    Year = currentYear,
                    TotalIncome = transactions.OfType<Income>().Sum(i => i.Amount),
                    TotalExpenses = transactions.OfType<Expense>().Sum(e => e.Amount)
                };
                report.Balance = report.TotalIncome - report.TotalExpenses;
                
                var dialog = new SaveFileDialog
                {
                    Filter = "PDF fajlovi (*.pdf)|*.pdf",
                    DefaultExt = ".pdf",
                    FileName = $"Report_{currentYear}_{currentMonth:D2}.pdf"
                };
                
                if (dialog.ShowDialog() == true)
                {
                    _reportService.GenerateMonthlyReport(report, transactions, dialog.FileName);
                    MessageBox.Show("Izveštaj je uspešno generisan!", "Generisanje izveštaja",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri generisanju izveštaja: {ex.Message}", "Greška",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void Logout()
        {
            var result = MessageBox.Show(
                "Da li ste sigurni da želite da se odjavite?",
                "Potvrda odjave",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                UserSession.Instance.Logout();
                LogoutRequested?.Invoke();
            }
        }
    }
}
