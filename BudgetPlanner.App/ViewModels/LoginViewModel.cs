using BudgetPlanner.App.Commands;
using BudgetPlanner.App.Models;
using BudgetPlanner.App.Services;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Input;

namespace BudgetPlanner.App.ViewModels
{
    /// <summary>
    /// ViewModel za Login prozor.
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        private readonly Repository<User> _userRepository;
        private string _username = string.Empty;
        private string _password = string.Empty;
        private string _errorMessage = string.Empty;
        
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }
        
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
        
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }
        
        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }
        
        public event System.Action? LoginSuccessful;
        
        public LoginViewModel()
        {
            _userRepository = new Repository<User>();
            
            LoginCommand = new RelayCommand(async _ => await ExecuteLogin(), _ => CanExecuteLogin());
            RegisterCommand = new RelayCommand(async _ => await ExecuteRegister(), _ => CanExecuteLogin());
        }
        
        private bool CanExecuteLogin()
        {
            return !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
        }
        
        private async System.Threading.Tasks.Task ExecuteLogin()
        {
            try
            {
                // Prvo pronađi korisnika po username-u
                var users = await _userRepository.FindAsync(u => u.Username == Username);
                var foundUser = System.Linq.Enumerable.FirstOrDefault(users);

                if (foundUser != null && PasswordHasher.VerifyPassword(Password, foundUser.PasswordHash))
                {
                    UserSession.Instance.Login(foundUser);
                    LoginSuccessful?.Invoke();
                }
                else
                {
                    ErrorMessage = "Neispravno korisničko ime ili lozinka.";
                }
            }
            catch (System.Exception ex)
            {
                ErrorMessage = $"Greška pri prijavljivanju: {ex.Message}";
            }
        }
        
        private async System.Threading.Tasks.Task ExecuteRegister()
        {
            try
            {
                var existingUser = await _userRepository.FindAsync(u => u.Username == Username);

                if (System.Linq.Enumerable.Any(existingUser))
                {
                    ErrorMessage = "Korisničko ime već postoji.";
                    return;
                }

                var newUser = new User
                {
                    Username = Username,
                    PasswordHash = PasswordHasher.HashPassword(Password), // Sada koristi BCrypt!
                    Email = $"{Username}@budgetplanner.com",
                    FullName = Username
                };

                await _userRepository.AddAsync(newUser);
                UserSession.Instance.Login(newUser);
                LoginSuccessful?.Invoke();
            }
            catch (System.Exception ex)
            {
                ErrorMessage = $"Greška pri registraciji: {ex.Message}";
            }
        }
    }
}
