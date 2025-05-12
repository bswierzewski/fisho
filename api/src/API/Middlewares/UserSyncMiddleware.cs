using Application.Common.Interfaces;

namespace API.Middlewares;

public class UserSyncMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<UserSyncMiddleware> _logger;

    public UserSyncMiddleware(RequestDelegate next, ILogger<UserSyncMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, ICurrentUserService currentUserService)
    {
        if (context.User.Identity?.IsAuthenticated ?? false)
        {
            try
            {
                // Upewnij się, że użytkownik istnieje w naszej bazie i pobierz jego AppId.
                // To wywołanie stworzy/zaktualizuje użytkownika w naszej bazie.
                int appUserId = await currentUserService.EnsureUserExistsAndGetIdAsync(context.RequestAborted);

                _logger.LogInformation("User {ClerkUserId} synchronized/verified. App User ID: {AppUserId}",
                                       currentUserService.ClerkUserId, appUserId);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "UserSyncMiddleware: Unauthorized access - ClerkUserId missing, though user is authenticated. This should not happen.");
                // To nie powinno się zdarzyć, jeśli IsAuthenticated jest true i Clerk działa poprawnie.
                // Można rozważyć zwrócenie 401/403, ale standardowe middleware Clerk powinno to obsłużyć.
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UserSyncMiddleware: Error occurred during user synchronization.");
                // Nie przerywaj żądania, ale zaloguj błąd. Aplikacja może nadal działać
                // dla niektórych endpointów, które nie wymagają AppUserId, lub obsłuży to dalej.
            }
        }

        await _next(context);
    }
}
