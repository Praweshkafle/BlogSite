using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.FileManager
{
    public class FileManager : IFileManager
    {
        private IHostingEnvironment hostingEnvironment;

        public FileManager(IHostingEnvironment _hostingEnvironment)
        {
            hostingEnvironment = _hostingEnvironment;
        }

        public string getRootPath() => hostingEnvironment.WebRootPath;

        public bool isImageValid(string file_name)
        {
            var allowedExtensions = new[] { ".jpeg", ".png", ".jpg" };
            var extension = Path.GetExtension(file_name).ToLower();
            if (!allowedExtensions.Contains(extension))
                return false;
            return true;
        }

        public bool isExcelFileValid(string file_name)
        {
            var allowedExtensions = new[] { ".xlsx", ".xls" };
            var extension = Path.GetExtension(file_name).ToLower();
            if (!allowedExtensions.Contains(extension))
                return false;
            return true;
        }

        public string saveImageAndGetFileName(IFormFile file, string file_prefix = "")
        {
            if (!isImageValid(file.FileName))
            {
                throw new Exception("invalid Document format. Document must be an image.");
            }

            if (!isImageSizeLessThan1Mb(file))
                throw new Exception("Image size must be less than 1 MB.");
            Guid random = Guid.NewGuid();

            string file_name = "";
            if (string.IsNullOrWhiteSpace(file_prefix))
            {
                file_name = random + Path.GetFileNameWithoutExtension(file.FileName) + Path.GetExtension(file.FileName);
            }
            else
            {
                file_name = random + file_prefix + Path.GetExtension(file.FileName);
            }

            var filePath = getPathToImageFolder();
            filePath = Path.Combine(filePath, file_name);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return file_name;
        }

        public bool isImageSizeLessThan1Mb(IFormFile file)
        {
            if (file != null)
            {
                int maxFileSize = 1024 * 1024;
                if (file.Length <= maxFileSize)
                    return true;
            }
            return false;
        }

        public string saveFileAndGetFileName(IFormFile file, string file_prefix = "")
        {
            if (!isFileValid(file.FileName))
            {
                throw new Exception($"Invalid Document format.Allowed extensions are {string.Join(',', getAllowedFileNameExtensions())}.");
            }

            if (!isFileSizeLessThan3Mb(file))
            {
                throw new Exception("File size must be less than 3 MB.");
            }

            Random random = new Random();

            string file_name = "";
            if (string.IsNullOrWhiteSpace(file_prefix))
            {
                file_name = Path.GetFileNameWithoutExtension(file.FileName) + random.Next(1, 1232384943) + Path.GetExtension(file.FileName);
            }
            else
            {
                file_name = file_prefix + random.Next(1, 1232384943) + Path.GetExtension(file.FileName);
            }

            var filePath = getPathToImageFolder();
            filePath = Path.Combine(filePath, file_name);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return file_name;
        }

        private bool isFileSizeLessThan3Mb(IFormFile file)
        {
            if (file != null)
            {
                int maxFileSize = 3 * 1024 * 3 * 1024;
                return file.Length <= maxFileSize;
            }
            return true;
        }

        private string[] getAllowedFileNameExtensions()
        {
            return new[] { ".docx", ".doc", ".pdf", ".xls", ".xlsx", ".ppt", ".pptx", ".jpeg", ".png", ".jpg" };
        }

        private bool isFileValid(string fileName)
        {
            var allowedExtensions = getAllowedFileNameExtensions();
            var extension = Path.GetExtension(fileName).ToLower();

            return allowedExtensions.Contains(extension);
        }

        public string getPathToImageFolder()
        {
            string path = Path.Combine(hostingEnvironment.WebRootPath, "Images");
            path = Path.Combine(path, "Custom");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }

        public void deleteImage(string imageName, string path)
        {
            if (File.Exists(Path.Combine(path, imageName)))
            {
                File.Delete(Path.Combine(path, imageName));
            }
        }

        public string getPathToDatabaseFolder() => Path.Combine(hostingEnvironment.WebRootPath, "database_backup");
    }

}
