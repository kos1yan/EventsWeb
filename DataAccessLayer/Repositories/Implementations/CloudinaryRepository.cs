using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using DataAccessLayer.Entities.ConfigurationModels;

namespace DataAccessLayer.Repositories.Implementations
{
    public class CloudinaryRepository : ICloudinaryRepository
    {
        private readonly Cloudinary _cloundinary;

        public CloudinaryRepository(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
                );
            _cloundinary = new Cloudinary(acc);
        }

        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream)
                };
                uploadResult = await _cloundinary.UploadAsync(uploadParams);
            }

            return uploadResult;
        }

        public async Task DeletePhotoAsync(string publicUrl)
        {
            var publicId = publicUrl.Split('/').Last().Split('.')[0];
            var deleteParams = new DeletionParams(publicId);
            var rezult = await _cloundinary.DestroyAsync(deleteParams);
        }
    }
}
