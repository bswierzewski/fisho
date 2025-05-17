namespace Fishio.Application.Common.Interfaces;

/// <summary>
/// Provides access to information about the currently authenticated user.
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Gets the unique identifier of the user from the external identity provider (Clerk).
    /// Returns null if the user is not authenticated or the claim is not present.
    /// </summary>
    string? ClerkUserId { get; }

    /// <summary>
    /// Gets the unique identifier of the corresponding domain user.
    /// This ID is typically populated after the UserProvisioningMiddleware has run.
    /// Returns null if the user is not authenticated or not yet provisioned in the domain.
    /// </summary>
    int? UserId { get; }

    /// <summary>
    /// Gets a value indicating whether the current user is authenticated.
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// Asynchronously retrieves the domain <see cref="User"/> entity for the currently authenticated user.
    /// This method may also handle Just-In-Time (JIT) provisioning of the user in the domain database
    /// if they exist in Clerk but not locally.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the domain <see cref="User"/>
    /// entity, or null if the user is not authenticated or could not be provisioned.
    /// </returns>
    /// <remarks>
    /// It's recommended that the UserProvisioningMiddleware calls this method early in the request pipeline
    /// to ensure the user is available for subsequent operations.
    /// </remarks>
    Task<User?> GetOrProvisionDomainUserAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets the domain user ID after successful provisioning or retrieval.
    /// This is typically called by the UserProvisioningMiddleware or internally.
    /// </summary>
    /// <param name="domainUserId">The domain user ID.</param>
    void SetUserId(int domainUserId);
}
