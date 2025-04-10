using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace NZWalks.API.Repository
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        public CloudinaryService(IConfiguration configuration)
        {
            var account = new Account(
                configuration["CloudinarySettings:CloudName"],
                configuration["CloudinarySettings:ApiKey"],
                configuration["CloudinarySettings:ApiSecret"]);

            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "region-images", // optional folder name
                //ResourceType = "image",
                Type = "authenticated"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult.SecureUrl.ToString(); // return the image URL
        }

        public string GetPrivateImageUrl(string publicId)
        {
            var url = _cloudinary.Api.UrlImgUp
                .Signed(true)
                .Secure(true)
                .Type("authenticated")
                .BuildUrl(publicId); // or BuildImageTag if you want full <img> tag

            return url;
        }

    }
}
