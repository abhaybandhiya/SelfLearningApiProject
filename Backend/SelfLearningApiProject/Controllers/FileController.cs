using Microsoft.AspNetCore.Mvc;
using SelfLearningApiProject.Services;

namespace SelfLearningApiProject.Controllers
{
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        // Constructor me IFileService ko inject karte hain
        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file) // IFormFile parameter ke through uploaded file ko receive karte hain
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var fileName = await _fileService.UploadFileAsync(file); // FileService ka method call karte hain file upload karne ke liye
            return Ok(new { FileName = fileName }); // Uploaded file ka unique name return karte hain
        }

        [HttpGet("download/{fileName}")] // fileName ko route parameter ke through lete hain
        public async Task<IActionResult> Download(string fileName) // Specified file ko download karne ke liye
        {
            var fileBytes = await _fileService.DownloadFileAsync(fileName);

            if (fileBytes == null)
                return NotFound("File not found");

            return File(fileBytes, "application/octet-stream", fileName);
        }
    }
}
