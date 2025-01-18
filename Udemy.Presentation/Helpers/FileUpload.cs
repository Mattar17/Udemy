
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Udemy.Presentation.Helpers
{
    
    public class FileUpload
    {
        private readonly Cloudinary _cloudinary;

        public FileUpload(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }
        public string UploadImage(IFormFile file)
        {
            if (file is null || file.Length == 0)
                return "";

            try
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName , file.OpenReadStream())
                };

                var uploadResult = _cloudinary.Upload(uploadParams);

                return uploadResult.SecureUrl.ToString();
            }

            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string UploadVideo(IFormFile file)
        {
            if (file is null || file.Length == 0)
                return "";

            try
            {
                var uploadParams = new VideoUploadParams()
                {
                    File = new FileDescription(file.FileName , file.OpenReadStream())
                };

                var uploadResult = _cloudinary.Upload(uploadParams);

                return uploadResult.SecureUrl.ToString();
            }

            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
