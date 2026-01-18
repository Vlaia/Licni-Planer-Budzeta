using BudgetPlanner.App.ViewModels;
using System.Windows.Controls;

namespace BudgetPlanner.App.Views
{
    /// <summary>
    /// Interaction logic for BudgetView.xaml
    /// </summary>
    public partial class BudgetView : UserControl
    {
        public BudgetView()
        {
            InitializeComponent();
            DataContext = new BudgetViewModel();
        }
    }
}
