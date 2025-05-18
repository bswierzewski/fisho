using System.Security.Cryptography;

namespace Infrastructure.Services
{
    public class CompetitionTokenGenerator : ICompetitionTokenGenerator
    {
        private const int TokenLength = 32; // Długość tokenu w bajtach, da 64 znaki hex

        public string GenerateUniqueToken()
        {
            // Użyj RandomNumberGenerator dla kryptograficznie bezpiecznych losowych bajtów
            byte[] tokenData = RandomNumberGenerator.GetBytes(TokenLength);
            // Konwertuj na string hexadecymalny
            return Convert.ToHexString(tokenData).ToLowerInvariant();
        }
    }
}
