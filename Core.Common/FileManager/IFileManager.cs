using Microsoft.AspNetCore.Http;

namespace Core.Common.FileManager
{
    public interface IFileManager
    {
        bool isImageValid(string file_name);
        string saveFileAndGetFileName(IFormFile file, string file_prefix = "");
        string saveImageAndGetFileName(IFormFile file, string file_prefix = "");
        bool isExcelFileValid(string file_name);
        bool isImageSizeLessThan1Mb(IFormFile file);
        string getRootPath();
        string getPathToImageFolder();
        string getPathToDatabaseFolder();
        void deleteImage(string imageName, string path);
    }
}
