using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.Metrics;

[Route("file")]
[ApiController]
public class FileUploadController : ControllerBase
{
    private readonly IWebHostEnvironment _hostingEnvironment;
    private string _tempPath = "Access\\File";

    public FileUploadController(IWebHostEnvironment hostingEnvironment)
    {
        _hostingEnvironment = hostingEnvironment;
    }

    [HttpPost("upfile")]
    public IActionResult UploadFile([FromForm] List<IFormFile> files)
    {
        if (files == null || files.Count == 0) return BadRequest("Không có tệp nào được tải lên.");

        List<string> uploadFileNames = new List<string>();

        foreach (var file in files)
        {
            if (file != null && file.Length > 0)
            {
                string fileExtension = Path.GetExtension(file.FileName).ToLower();

                string originalTempPath = string.Concat(_tempPath, $"\\{fileExtension}");

                // Lay duong dan thu muc luu file
                string uploadFolder = Path.Combine(_hostingEnvironment.ContentRootPath, originalTempPath);

                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);

                    string fileName = Path.GetFileName(file.FileName);

                    string filePath = Path.Combine(uploadFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    uploadFileNames.Add(fileName);
                }
                else
                {
                    string fileName = Path.GetFileName(file.FileName);

                    string newNameFile = GetUniqueFileName(uploadFolder, fileName);

                    if (fileName.Equals(newNameFile))
                    {
                        string filePath = Path.Combine(uploadFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyToAsync(stream);
                        }

                        uploadFileNames.Add(fileName);
                    }
                    else
                    {
                        string filePath = Path.Combine(uploadFolder, newNameFile);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyToAsync(stream);
                        }

                        uploadFileNames.Add(newNameFile);
                    }
                    
                }
                originalTempPath = _tempPath;
            }
        }

        return Ok($"Đã tải lên {uploadFileNames.Count} tệp thành công: {string.Join(", ", uploadFileNames)}");
    }

    private string GetUniqueFileName(string folderPath, string fileName)
    {
        string newFileName = fileName;
        string fileExtension = Path.GetExtension(fileName);
        int count = 0;
        List<string> fileNameCurrent = GetAllFileNames(folderPath);

        foreach(string file in fileNameCurrent)
        {
            if(file.Equals(fileName))
            {
                count++;
            }   
        }

        if(count == 0)
        {
            return newFileName;
        }

        newFileName = $"{Path.GetFileNameWithoutExtension(fileName)}({count}){fileExtension}";

        return newFileName;

    }

    [HttpGet("downfile/{fileName}")]
    public IActionResult DownloadFile(string fileName)
    {
        string folderPath = Path.Combine(_hostingEnvironment.ContentRootPath, _tempPath);
        string filePath = SearchFile(folderPath, fileName);

        if (filePath.IsNullOrEmpty())
        {
            return BadRequest("Không tìm thấy file cần tải");
        }

        // Đọc nội dung tệp
        var fileBytes = System.IO.File.ReadAllBytes(filePath);

        // Trả về tệp tải xuống cho người dùng
        return File(fileBytes, "application/octet-stream", fileName);
    }

    private string SearchFile(string directoryPath, string fileName)
    {
        if (Directory.Exists(directoryPath))
        {
            string[] filesInDirectory = Directory.GetFiles(directoryPath, fileName);

            if (filesInDirectory.Length > 0)
            {
                return filesInDirectory[0]; // Trả về đường dẫn đến tệp nếu tìm thấy
            }

            // Lấy tất cả thư mục con
            string[] subDirectories = Directory.GetDirectories(directoryPath);

            foreach (string subDirectory in subDirectories)
            {
                string filePath = SearchFile(subDirectory, fileName);
                if (!string.IsNullOrEmpty(filePath))
                {
                    return filePath; // Nếu tìm thấy trong thư mục con, trả về luôn
                }
            }
        }

        return null; // Trả về null nếu không tìm thấy
    }


    [HttpGet("getfile")]
    public IActionResult GetAllFile()
    {
        string folderPath = Path.Combine(_hostingEnvironment.ContentRootPath, _tempPath);

        List<string> allFileNames = GetAllFileNames(folderPath);

        return Ok(allFileNames);
    }

    private List<string> GetAllFileNames(string path)
    {
        List<string> fileNames = new List<string>();

        if (Directory.Exists(path))
        {
            // Lấy tất cả tên tệp trong thư mục hiện tại
            string[] filesInCurrentDirectory = Directory.GetFiles(path);
            foreach (string filePath in filesInCurrentDirectory)
            {
                fileNames.Add(Path.GetFileName(filePath));
            }

            // Lấy tất cả tệp trong các thư mục con bằng đệ quy
            string[] subDirectories = Directory.GetDirectories(path);
            foreach (string subDirectory in subDirectories)
            {
                List<string> subDirectoryFiles = GetAllFileNames(subDirectory);
                fileNames.AddRange(subDirectoryFiles);
            }
        }

        return fileNames;
    }


}
