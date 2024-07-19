using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace DataAccessLayer.Repositories
{
    public interface ICloudinaryRepository
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);

        Task DeletePhotoAsync(string publicUrl);
    }
}
