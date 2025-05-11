namespace Application.Common.Interfaces;

public interface IUser
{
    /// <summary>
    /// Nasze wewnętrzne ID użytkownika z tabeli Users (po zmapowaniu)
    /// </summary>
    int? Id { get; }

    /// <summary>
    /// Bezpośrednie ID z tokenu Clerk (zazwyczaj 'sub')
    /// </summary>
    string? ClerkUserId { get; } 

    /// <summary>
    /// 
    /// </summary>
    string? Email { get; }
}
