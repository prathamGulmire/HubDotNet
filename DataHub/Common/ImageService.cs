namespace DataHub.Common
{
    public class ImageService
    {
        private readonly string _uploadPath;

        public ImageService()
        {
            _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
        }

        public string SaveImage(IFormFile image, string prefix)
        {
            string extension = Path.GetExtension(image.FileName).ToLower();

            if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
                throw new Exception("Invalid image format");

            string fileName = prefix + "_" + Guid.NewGuid() + extension;
            string fullPath = Path.Combine(_uploadPath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                image.CopyTo(stream);
            }

            return fileName;
        }

        public bool DeleteImage(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return false;

            string fullPath = Path.Combine(_uploadPath, fileName);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return true;
            }

            return false;    
        }
    }
}
