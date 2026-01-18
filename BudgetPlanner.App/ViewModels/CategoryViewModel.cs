using BudgetPlanner.App.Commands;
using BudgetPlanner.App.Helpers;
using BudgetPlanner.App.Models;
using BudgetPlanner.App.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace BudgetPlanner.App.ViewModels
{
    /// <summary>
    /// ViewModel za upravljanje kategorijama.
    /// </summary>
    public class CategoryViewModel : ViewModelBase
    {
        private readonly Repository<Category> _categoryRepository;
        
        private ObservableCollection<Category> _categories = new();
        private Category? _selectedCategory;
        private string _categoryType = "Expense";
        private string _name = string.Empty;
        private string _description = string.Empty;
        private string _color = "#808080";
        
        public ObservableCollection<Category> Categories
        {
            get => _categories;
            set => SetProperty(ref _categories, value);
        }
        
        public Category? SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (SetProperty(ref _selectedCategory, value) && value != null)
                {
                    LoadCategoryDetails(value);
                }
            }
        }
        
        public string CategoryType
        {
            get => _categoryType;
            set => SetProperty(ref _categoryType, value);
        }
        
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }
        
        public string Color
        {
            get => _color;
            set => SetProperty(ref _color, value);
        }
        
        public ICommand AddCategoryCommand { get; }
        public ICommand UpdateCategoryCommand { get; }
        public ICommand DeleteCategoryCommand { get; }
        public ICommand ClearFormCommand { get; }
        
        public CategoryViewModel()
        {
            _categoryRepository = new Repository<Category>();
            
            AddCategoryCommand = new RelayCommand(async _ => await AddCategory(), _ => CanAddCategory());
            UpdateCategoryCommand = new RelayCommand(async _ => await UpdateCategory(), _ => SelectedCategory != null);
            DeleteCategoryCommand = new RelayCommand(async _ => await DeleteCategory(), _ => SelectedCategory != null);
            ClearFormCommand = new RelayCommand(_ => ClearForm());
            
            LoadCategories();
        }
        
        private bool CanAddCategory()
        {
            return !string.IsNullOrWhiteSpace(Name);
        }
        
        private async System.Threading.Tasks.Task AddCategory()
        {
            try
            {
                // Validacija
                if (!ValidationHelper.IsNotEmpty(Name))
                {
                    MessageBox.Show("Naziv ne sme biti prazan!", "Validaciona greška",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!ValidationHelper.IsValidHexColor(Color))
                {
                    MessageBox.Show("Boja mora biti u hex formatu (npr. #FF6B6B)!", "Validaciona greška",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Category category;

                if (CategoryType == "Income")
                {
                    category = new IncomeCategory
                    {
                        Name = Name,
                        Description = Description,
                        Color = Color,
                        UserId = UserSession.Instance.CurrentUser!.Id,
                        IsRecurring = false
                    };
                }
                else
                {
                    category = new ExpenseCategory
                    {
                        Name = Name,
                        Description = Description,
                        Color = Color,
                        UserId = UserSession.Instance.CurrentUser!.Id,
                        IsEssential = false,
                        MaxMonthlyBudget = 0
                    };
                }

                await _categoryRepository.AddAsync(category);
                await LoadCategories();
                ClearForm();

                MessageBox.Show("Kategorija je uspešno dodata!", "Uspeh",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri dodavanju kategorije: {ex.Message}", "Greška",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private async System.Threading.Tasks.Task UpdateCategory()
        {
            if (SelectedCategory == null) return;
            
            try
            {
                SelectedCategory.Name = Name;
                SelectedCategory.Description = Description;
                SelectedCategory.Color = Color;
                
                await _categoryRepository.UpdateAsync(SelectedCategory);
                await LoadCategories();
                ClearForm();
                
                MessageBox.Show("Kategorija je uspešno ažurirana!", "Uspeh",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri ažuriranju kategorije: {ex.Message}", "Greška",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private async System.Threading.Tasks.Task DeleteCategory()
        {
            if (SelectedCategory == null) return;
            
            var result = MessageBox.Show(
                "Da li ste sigurni da želite da obrišete ovu kategoriju?",
                "Potvrda brisanja",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await _categoryRepository.DeleteAsync(SelectedCategory);
                    await LoadCategories();
                    ClearForm();
                    
                    MessageBox.Show("Kategorija je uspešno obrisana!", "Uspeh",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Greška pri brisanju kategorije: {ex.Message}", "Greška",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        
        private void ClearForm()
        {
            SelectedCategory = null;
            Name = string.Empty;
            Description = string.Empty;
            Color = "#808080";
            CategoryType = "Expense";
        }
        
        private void LoadCategoryDetails(Category category)
        {
            Name = category.Name;
            Description = category.Description;
            Color = category.Color;
            CategoryType = category.GetCategoryType();
        }
        
        private async System.Threading.Tasks.Task LoadCategories()
        {
            try
            {
                var userId = UserSession.Instance.CurrentUser!.Id;
                var categories = await _categoryRepository.FindAsync(c => c.UserId == userId);
                
                Categories.Clear();
                foreach (var category in categories)
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
    }
}
