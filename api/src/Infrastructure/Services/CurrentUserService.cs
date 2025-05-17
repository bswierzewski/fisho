// Fishio.Infrastructure/Services/CurrentUserService.cs
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Fishio.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IApplicationDbContext _dbContext;
    private readonly ILogger<CurrentUserService> _logger;
    private int? _cachedDomainUserId;

    public CurrentUserService(
        IHttpContextAccessor httpContextAccessor,
        IApplicationDbContext dbContext,
        ILogger<CurrentUserService> logger)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public string? ClerkUserId
    {
        get
        {
            var principal = _httpContextAccessor.HttpContext?.User;
            // Clerk często używa claimu 'sub' jako głównego identyfikatora użytkownika.
            // ClaimTypes.NameIdentifier jest również standardowym miejscem.
            // Sprawdź dokumentację Clerk, który claim zawiera unikalne ID użytkownika.
            return principal?.FindFirst("sub")?.Value // Preferowany dla OpenID Connect
                   ?? principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    public int? UserId => _cachedDomainUserId;

    public void SetDomainUserId(int domainUserId)
    {
        _cachedDomainUserId = domainUserId;
    }

    public async Task<User?> GetOrProvisionDomainUserAsync(CancellationToken cancellationToken = default)
    {
        if (!IsAuthenticated)
        {
            _logger.LogDebug("User is not authenticated (JWT validation likely failed or token not present). Cannot get or provision domain user.");
            return null;
        }

        var currentClerkUserId = ClerkUserId;
        if (string.IsNullOrEmpty(currentClerkUserId))
        {
            _logger.LogWarning("User is authenticated via JWT, but ClerkUserId claim ('sub' or NameIdentifier) is missing or empty.");
            return null;
        }

        var domainUser = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.ClerkUserId == currentClerkUserId, cancellationToken);

        if (domainUser != null)
        {
            SetDomainUserId(domainUser.Id);
            _logger.LogDebug("Found existing domain user {DomainUserId} for ClerkUserId {ClerkUserId} (from JWT).", domainUser.Id, currentClerkUserId);
            return domainUser;
        }

        _logger.LogInformation("Domain user not found for ClerkUserId {ClerkUserId} (from JWT). Attempting JIT provisioning.", currentClerkUserId);

        var principal = _httpContextAccessor.HttpContext?.User;
        // Przykładowe pobieranie imienia i emaila z claimów JWT.
        // Nazwy claimów mogą się różnić w zależności od konfiguracji Clerk.
        var userName = principal?.FindFirst(ClaimTypes.Name)?.Value
                       ?? principal?.FindFirst("name")?.Value
                       ?? principal?.FindFirst("preferred_username")?.Value
                       ?? "Clerk User";
        var userEmail = principal?.FindFirst(ClaimTypes.Email)?.Value
                        ?? principal?.FindFirst("email")?.Value;

        var newDomainUser = new User(currentClerkUserId, userName, userEmail);

        try
        {
            _dbContext.Users.Add(newDomainUser);
            await _dbContext.SaveChangesAsync(cancellationToken);

            SetDomainUserId(newDomainUser.Id);
            _logger.LogInformation("Successfully provisioned new domain user {DomainUserId} for ClerkUserId {ClerkUserId} (from JWT).", newDomainUser.Id, currentClerkUserId);
            return newDomainUser;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during JIT provisioning for ClerkUserId {ClerkUserId} (from JWT).", currentClerkUserId);
            return null;
        }
    }
}
