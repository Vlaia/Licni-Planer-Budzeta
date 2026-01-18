using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BudgetPlanner.App.Helpers
{
    /// <summary>
    /// Konverter koji konvertuje string u Visibility.
    /// Vraća Visible ako string nije prazan, inače Collapsed.
    /// </summary>
    public class StringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str && !string.IsNullOrWhiteSpace(str))
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }
        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
