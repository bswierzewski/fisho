namespace Application.Common.Interfaces;

public interface ICurrentUserService
{
    /// <summary>
    /// Unikalny identyfikator użytkownika z systemu Clerk (zazwyczaj 'sub' z tokenu JWT).
    /// Może być null, jeśli użytkownik nie jest uwierzytelniony.
    /// </summary>
    string? ClerkUserId { get; }

    /// <summary>
    /// Wewnętrzny, numeryczny identyfikator użytkownika z bazy danych aplikacji Fisho.
    /// Może być null, jeśli użytkownik nie jest uwierzytelniony lub nie został jeszcze zsynchronizowany.
    /// Aby zagwarantować jego istnienie dla uwierzytelnionego użytkownika, użyj EnsureUserExistsAndGetIdAsync.
    /// </summary>
    int? Id { get; }

    /// <summary>
    /// Adres email użytkownika pobrany z tokenu Clerk.
    /// Może być null.
    /// </summary>
    string? Email { get; }

    /// <summary>
    /// Nazwa wyświetlana użytkownika pobrana z tokenu Clerk.
    /// Może być null.
    /// </summary>
    string? NameFromToken { get; }

    /// <summary>
    /// Sprawdza, czy użytkownik istnieje w lokalnej bazie danych na podstawie ClerkUserId.
    /// Jeśli nie istnieje, tworzy go. Jeśli istnieje, opcjonalnie aktualizuje jego dane (imię, email).
    /// Zwraca wewnętrzne, numeryczne ID użytkownika aplikacji Fisho.
    /// Rzuca UnauthorizedAccessException, jeśli ClerkUserId nie jest dostępny (użytkownik nie jest uwierzytelniony).
    /// </summary>
    Task<int> EnsureUserExistsAndGetIdAsync(CancellationToken cancellationToken = default);
}
