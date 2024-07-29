using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Events.Application.Interfaces
{
    public interface ICloudinaryRepository
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file, CancellationToken token);

        Task DeletePhotoAsync(string publicUrl);
    }
}
