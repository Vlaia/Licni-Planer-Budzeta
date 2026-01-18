using BudgetPlanner.App.ViewModels;
using System.Windows;

namespace BudgetPlanner.App.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            
            if (DataContext is MainViewModel viewModel)
            {
                viewModel.LogoutRequested += OnLogoutRequested;
            }
        }
        
        private void OnLogoutRequested()
        {
            var loginView = new LoginView();
            loginView.Show();
            this.Close();
        }
    }
}
