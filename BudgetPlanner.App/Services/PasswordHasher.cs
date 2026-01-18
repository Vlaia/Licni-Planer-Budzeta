using BCrypt.Net;

namespace BudgetPlanner.App.Services
{
    /// <summary>
    /// Servis za heširanje i verifikaciju lozinki koristeći BCrypt.
    /// </summary>
    public static class PasswordHasher
    {
        /// <summary>
        /// Heširaj lozinku koristeći BCrypt algoritam.
        /// </summary>
        /// <param name="password">Plain text lozinka</param>
        /// <returns>Heširana lozinka</returns>
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
        }

        /// <summary>
        /// Verifikuj da li plain text lozinka odgovara heširanoj.
        /// </summary>
        /// <param name="password">Plain text lozinka</param>
        /// <param name="hashedPassword">Heširana lozinka iz baze</param>
        /// <returns>True ako se poklapaju, inače False</returns>
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            }
            catch
            {
                // Ako hash nije validan BCrypt format, probaj sa plain text (za migraciju)
                return password == hashedPassword;
            }
        }
    }
}
