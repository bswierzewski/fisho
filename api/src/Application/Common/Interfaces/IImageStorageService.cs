namespace Application.Common.Interfaces.Services;

public interface IImageStorageService
{
    /// <summary>
    /// Uploads an image to the storage service.
    /// </summary>
    /// <param name="imageStream">The stream containing the image data.</param>
    /// <param name="fileName">The original file name of the image.</param>
    /// <param name="folderName">Optional: The name of the folder in the storage where the image should be placed.</param>
    /// <returns>An <see cref="ImageUploadResult"/> indicating the outcome of the upload.</returns>
    Task<ImageUploadResult> UploadImageAsync(Stream imageStream, string fileName, string? folderName = null);

    /// <summary>
    /// Deletes an image from the storage service using its public identifier.
    /// </summary>
    /// <param name="publicId">The public identifier of the image to delete (e.g., Cloudinary Public ID).</param>
    /// <returns>True if the deletion was successful or the image was not found; otherwise, false.</returns>
    Task<bool> DeleteImageAsync(string publicId);
}
