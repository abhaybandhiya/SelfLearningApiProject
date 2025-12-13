namespace SelfLearningApiProject.Services
{
    // Yeh class IFileService interface ko implement karti hai - file related operations ke liye
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        public FileService(IWebHostEnvironment env, IConfiguration config)
        {
            _env = env;
            _config = config;
        }

        // File upload karne ka method - yeh file ko server par save karta hai aur unique file name return karta hai
        public async Task<string> UploadFileAsync(IFormFile file)
        {
            // Configuration se upload path lete hain FileSettings:UploadPath appsettings.json me defined hai
            string folder = _config["FileSettings:UploadPath"];

            // Full path banate hain jahan file save hogi
            string fullPath = Path.Combine(_env.ContentRootPath, folder);

            // Agar directory exist nahi karti to create karte hain
            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);

            // Unique file name generate karte hain taaki naam clash na ho
            string fileName = file.FileName + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
            string filePath = Path.Combine(fullPath, fileName); // Full file path jahan file save hogi

            using (var stream = new FileStream(filePath, FileMode.Create)) // FileStream banate hain jisme file write hogi
            {
                await file.CopyToAsync(stream); // File ko stream me copy karte hain asynchronously
            }

            return fileName; // return unique name jo client ko diya jayega taaki wo file download kar sake 
        }
        // File download karne ka method - yeh specified file ko read karta hai aur byte array return karta hai
        public async Task<byte[]> DownloadFileAsync(string fileName) 
        {
            string folder = _config["FileSettings:UploadPath"];
            string filePath = Path.Combine(_env.ContentRootPath, folder, fileName);

            if (!File.Exists(filePath))
                return null;

            return await File.ReadAllBytesAsync(filePath);
        }
    }
}
