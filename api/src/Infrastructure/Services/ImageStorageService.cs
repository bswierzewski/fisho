using Application.Common.Interfaces.Services; // Dodano dla logowania
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Fishio.Infrastructure.Services;

public class ImageStorageService : IImageStorageService
{
    private readonly Cloudinary _cloudinary;
    private readonly ILogger<ImageStorageService> _logger;

    public ImageStorageService(IConfiguration configuration, ILogger<ImageStorageService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        var cloudName = configuration["Cloudinary:CloudName"];
        var apiKey = configuration["Cloudinary:ApiKey"];
        var apiSecret = configuration["Cloudinary:ApiSecret"];

        if (string.IsNullOrWhiteSpace(cloudName) ||
            string.IsNullOrWhiteSpace(apiKey) ||
            string.IsNullOrWhiteSpace(apiSecret))
        {
            _logger.LogError("Cloudinary configuration (CloudName, ApiKey, ApiSecret) is missing or incomplete in application settings.");
            throw new ArgumentNullException(nameof(configuration), "Cloudinary configuration is missing or incomplete.");
        }

        Account account = new Account(cloudName, apiKey, apiSecret);
        _cloudinary = new Cloudinary(account);
        _logger.LogInformation("ImageStorageService (Cloudinary) initialized for cloud: {CloudName}", cloudName);
    }

    public async Task<Application.Common.Models.ImageUploadResult> UploadImageAsync(Stream imageStream, string fileName, string? folderName = null)
    {
        if (imageStream == null || imageStream.Length == 0)
        {
            _logger.LogWarning("UploadImageAsync called with null or empty imageStream for fileName: {FileName}", fileName);
            return new Application.Common.Models.ImageUploadResult { Success = false, ErrorMessage = "Image stream cannot be null or empty." };
        }
        if (string.IsNullOrWhiteSpace(fileName))
        {
            _logger.LogWarning("UploadImageAsync called with null or empty fileName.");
            return new Application.Common.Models.ImageUploadResult { Success = false, ErrorMessage = "File name cannot be null or empty." };
        }

        // Użyj Guid do generowania unikalnej części nazwy pliku, aby uniknąć konfliktów,
        // zachowując oryginalne rozszerzenie.
        var uniqueFileName = $"{Path.GetFileNameWithoutExtension(fileName)}_{Guid.NewGuid()}{Path.GetExtension(fileName)}";

        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(uniqueFileName, imageStream),
            // PublicId można ustawić, jeśli chcemy mieć pełną kontrolę nad identyfikatorem w Cloudinary.
            // Jeśli nie ustawimy, Cloudinary wygeneruje go automatycznie (często na podstawie nazwy pliku).
            // PublicId = uniqueFileName, // Można odkomentować, jeśli chcemy używać naszej unikalnej nazwy jako PublicId
            Folder = folderName, // Opcjonalny folder w Cloudinary
            Overwrite = true, // Czy nadpisywać, jeśli plik o tym samym PublicId istnieje
            // Można dodać tagi, transformacje itp.
            // EagerTransforms = new List<Transformation>() { new Transformation().Width(500).Height(500).Crop("fill").Gravity("face") }
        };

        _logger.LogInformation("Attempting to upload image {FileName} to Cloudinary. Folder: {Folder}", uniqueFileName, folderName ?? "root");

        try
        {
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
            {
                _logger.LogError("Cloudinary upload failed for {FileName}. Error: {ErrorMessage}. HTTP Status Code: {StatusCode}",
                    uniqueFileName, uploadResult.Error.Message, uploadResult.StatusCode);
                return new Application.Common.Models.ImageUploadResult { Success = false, ErrorMessage = uploadResult.Error.Message };
            }

            _logger.LogInformation("Successfully uploaded image {FileName} to Cloudinary. URL: {Url}, PublicId: {PublicId}",
                uniqueFileName, uploadResult.SecureUrl?.ToString() ?? uploadResult.Url?.ToString(), uploadResult.PublicId);

            return new Application.Common.Models.ImageUploadResult
            {
                Success = true,
                Url = uploadResult.SecureUrl?.ToString() ?? uploadResult.Url?.ToString(),
                PublicId = uploadResult.PublicId
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred during Cloudinary upload for {FileName}.", uniqueFileName);
            return new Application.Common.Models.ImageUploadResult { Success = false, ErrorMessage = $"An unexpected error occurred: {ex.Message}" };
        }
    }

    public async Task<bool> DeleteImageAsync(string publicId)
    {
        if (string.IsNullOrWhiteSpace(publicId))
        {
            _logger.LogWarning("DeleteImageAsync called with null or empty publicId.");
            return false; // Lub rzucić ArgumentNullException, w zależności od kontraktu
        }

        var deletionParams = new DeletionParams(publicId)
        {
            ResourceType = ResourceType.Image // Upewnij się, że usuwasz obraz
        };

        _logger.LogInformation("Attempting to delete image with PublicId: {PublicId} from Cloudinary.", publicId);

        try
        {
            var result = await _cloudinary.DestroyAsync(deletionParams);

            // "ok" oznacza sukces, "not found" oznacza, że pliku już nie ma (co też jest sukcesem w kontekście usunięcia)
            if (result.Result.Equals("ok", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogInformation("Successfully deleted image with PublicId: {PublicId} from Cloudinary.", publicId);
                return true;
            }
            if (result.Result.Equals("not found", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogInformation("Image with PublicId: {PublicId} not found in Cloudinary (considered as successful deletion).", publicId);
                return true;
            }

            _logger.LogWarning("Failed to delete image with PublicId: {PublicId} from Cloudinary. Result: {DeletionResult}", publicId, result.Result);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred during Cloudinary deletion for PublicId: {PublicId}.", publicId);
            return false;
        }
    }
}
