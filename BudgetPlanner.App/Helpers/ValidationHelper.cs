using System.Text.RegularExpressions;

namespace BudgetPlanner.App.Helpers
{
    /// <summary>
    /// Helper klasa za validaciju unosa.
    /// </summary>
    public static class ValidationHelper
    {
        /// <summary>
        /// Validira email adresu.
        /// </summary>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // RFC 5322 Official Standard email regex (simplified)
                var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                return Regex.IsMatch(email, emailPattern, RegexOptions.IgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Validira hex boju (#RRGGBB format).
        /// </summary>
        public static bool IsValidHexColor(string color)
        {
            if (string.IsNullOrWhiteSpace(color))
                return false;

            // Proverava #RRGGBB ili #RGB format
            var hexPattern = @"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$";
            return Regex.IsMatch(color, hexPattern);
        }

        /// <summary>
        /// Validira da li je iznos pozitivan.
        /// </summary>
        public static bool IsPositiveAmount(decimal amount)
        {
            return amount > 0;
        }

        /// <summary>
        /// Validira da li je string prazan ili sadrži samo whitespace.
        /// </summary>
        public static bool IsNotEmpty(string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }
    }
}
