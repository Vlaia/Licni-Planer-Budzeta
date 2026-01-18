using BudgetPlanner.App.ViewModels;
using System.Windows;

namespace BudgetPlanner.App.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            
            if (DataContext is LoginViewModel viewModel)
            {
                viewModel.LoginSuccessful += OnLoginSuccessful;
            }
        }
        
        private void OnLoginSuccessful()
        {
            var mainView = new MainView();
            mainView.Show();
            this.Close();
        }
        
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel viewModel)
            {
                viewModel.Password = PasswordBox.Password;
            }
        }
    }
}
