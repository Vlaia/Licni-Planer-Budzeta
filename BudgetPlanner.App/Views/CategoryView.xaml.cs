using BudgetPlanner.App.ViewModels;
using System.Windows.Controls;

namespace BudgetPlanner.App.Views
{
    /// <summary>
    /// Interaction logic for CategoryView.xaml
    /// </summary>
    public partial class CategoryView : UserControl
    {
        public CategoryView()
        {
            InitializeComponent();
            DataContext = new CategoryViewModel();
        }
    }
}
