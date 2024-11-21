
namespace Udemy.Presentation.Helpers
{
    public class FileUpload
    {
        private static readonly string _baseUploadPath = "wwwroot/uploads/";

        public FileUpload()
        {
            if (!Directory.Exists(_baseUploadPath))
            {
                Directory.CreateDirectory(_baseUploadPath);
            }

        }

        public static async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return "File cannot be null or empty";
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath  = Path.Combine(_baseUploadPath, fileName);

            using (var stream = new FileStream(filePath , FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fileUrl = $"http://localhost:7100/uploads/{fileName}";
            return fileUrl;

        }
    }
}
