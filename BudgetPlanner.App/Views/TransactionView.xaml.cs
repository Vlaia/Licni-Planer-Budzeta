using BudgetPlanner.App.ViewModels;
using System.Windows.Controls;

namespace BudgetPlanner.App.Views
{
    /// <summary>
    /// Interaction logic for TransactionView.xaml
    /// </summary>
    public partial class TransactionView : UserControl
    {
        public TransactionView()
        {
            InitializeComponent();
            DataContext = new TransactionViewModel();
        }
    }
}
