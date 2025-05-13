using System.Security.Claims;
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IApplicationDbContext _dbContext; // Potrzebne do operacji na bazie

        public CurrentUserService(IHttpContextAccessor httpContextAccessor, IApplicationDbContext dbContext)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public string? ClerkUserId =>
            _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? // Standardowy claim dla ID
            _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value; // 'sub' jest typowe dla Clerk ID

        // Ta właściwość zwraca ID użytkownika z bazy, jeśli został już zmapowany w ramach bieżącego żądania.
        // Dla pewności, że użytkownik istnieje i mamy jego ID, należy użyć EnsureUserExistsAndGetIdAsync.
        public int? Id
        {
            get
            {
                var clerkId = ClerkUserId;
                if (string.IsNullOrEmpty(clerkId))
                {
                    return null;
                }
                // To jest synchroniczne i może być problematyczne.
                // Lepszym wzorcem jest poleganie na EnsureUserExistsAndGetIdAsync.
                // Można by tu dodać logikę cachowania per żądanie, jeśli to ID jest często odczytywane.
                var user = _dbContext.Users.AsNoTracking().FirstOrDefault(u => u.ClerkUserId == clerkId);
                return user?.Id;
            }
        }

        public string? Email =>
            _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;

        public string? NameFromToken =>
            _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? // Może być imię i nazwisko
            _httpContextAccessor.HttpContext?.User?.FindFirst("name")?.Value ??
            _httpContextAccessor.HttpContext?.User?.FindFirst("preferred_username")?.Value ??
            _httpContextAccessor.HttpContext?.User?.FindFirst("given_name")?.Value; // Spróbuj różnych popularnych claimów

        public async Task<int> EnsureUserExistsAndGetIdAsync(CancellationToken cancellationToken = default)
        {
            var clerkId = ClerkUserId;
            if (string.IsNullOrEmpty(clerkId))
            {
                throw new UnauthorizedAccessException("Brak identyfikatora użytkownika Clerk w tokenie. Użytkownik nie jest uwierzytelniony.");
            }

            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.ClerkUserId == clerkId, cancellationToken);

            var nameFromToken = NameFromToken ?? $"Użytkownik_{clerkId.Substring(0, Math.Min(clerkId.Length, 8))}"; // Fallback dla nazwy
            var emailFromToken = Email;

            if (user == null)
            {
                // Użytkownik nie istnieje, stwórz go używając metody fabrycznej z encji
                user = new User(clerkId, nameFromToken, emailFromToken);
                _dbContext.Users.Add(user);
            }
            else            
                user.UpdateDetailsFromClerk(nameFromToken, emailFromToken);            

            await _dbContext.SaveChangesAsync(cancellationToken);
            return user.Id;
        }
    }
}
