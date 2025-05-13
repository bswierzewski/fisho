using Application.Common.Interfaces;

namespace API.Middlewares;

/// <summary>
/// Middleware to ensure that an authenticated user from Clerk
/// is provisioned or available as a domain user.
/// This should be placed after authentication middleware and before authorization or endpoint execution.
/// </summary>
public class UserProvisioningMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<UserProvisioningMiddleware> _logger;

    public UserProvisioningMiddleware(RequestDelegate next, ILogger<UserProvisioningMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InvokeAsync(HttpContext context, ICurrentUserService currentUserService)
    {
        // Sprawdź, czy użytkownik jest uwierzytelniony przez poprzednie middleware (np. Clerk JWT)
        if (context.User?.Identity?.IsAuthenticated == true)
        {
            _logger.LogDebug("User is authenticated. Attempting to get or provision domain user for ClerkUserId: {ClerkUserId}", currentUserService.ClerkUserId);
            try
            {
                var domainUser = await currentUserService.GetOrProvisionDomainUserAsync(context.RequestAborted);
                if (domainUser != null)
                {
                    // currentUserService.SetDomainUserId(domainUser.Id); // Już ustawiane wewnątrz GetOrProvisionDomainUserAsync
                    _logger.LogDebug("Domain user {DomainUserId} successfully retrieved or provisioned.", domainUser.Id);
                }
                else if (!string.IsNullOrEmpty(currentUserService.ClerkUserId)) // Jeśli był ClerkUserId, ale nie udało się zmapować
                {
                    // Ten scenariusz może oznaczać problem z provisioningiem lub konfiguracją.
                    // W zależności od polityki aplikacji, można tu podjąć różne działania:
                    // 1. Zalogować błąd i kontynuować (użytkownik może nie mieć dostępu do zasobów wymagających DomainUserId).
                    // 2. Zwrócić błąd HTTP 500 lub 403/401, jeśli użytkownik domenowy jest absolutnie wymagany.
                    _logger.LogWarning("Failed to retrieve or provision domain user for authenticated ClerkUserId: {ClerkUserId}. DomainUserId will be null.", currentUserService.ClerkUserId);
                }
            }
            catch (Exception ex)
            {
                // Logowanie krytycznego błędu podczas próby provisioningu
                _logger.LogError(ex, "An unhandled exception occurred during user provisioning for ClerkUserId: {ClerkUserId}.", currentUserService.ClerkUserId);
                // W środowisku produkcyjnym, nie chcesz, aby surowy wyjątek dotarł do klienta.
                // Globalny ExceptionHandlingMiddleware powinien to przechwycić.
                // Można rozważyć rzucenie specyficznego wyjątku lub zwrócenie błędu HTTP.
                // Na przykład, jeśli provisioning jest krytyczny:
                // context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                // await context.Response.WriteAsync("An error occurred while processing your user information.");
                // return; // Przerwij dalsze przetwarzanie
                throw; // Pozwól globalnemu handlerowi błędów to obsłużyć
            }
        }
        else
        {
            _logger.LogTrace("User is not authenticated. Skipping domain user provisioning.");
        }

        await _next(context);
    }
}