namespace Fishio.Application.Common.Models;

public class ImageUploadResult
{
    public bool Success { get; set; }
    public string? Url { get; set; }
    public string? PublicId { get; set; } // Np. dla Cloudinary
    public string? ErrorMessage { get; set; }
}