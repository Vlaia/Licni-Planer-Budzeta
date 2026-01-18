using System.Windows;
using BudgetPlanner.App.Data;

namespace BudgetPlanner.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Inicijalizuj bazu podataka
            var context = BudgetDbContext.Instance;
            DbInitializer.Initialize(context);
        }
    }
}
