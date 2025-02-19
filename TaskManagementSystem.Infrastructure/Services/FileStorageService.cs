using System.IO;
using System.Threading.Tasks;

namespace TaskManagementSystem.Infrastructure.Services
{
    public interface IFileStorageService
    {
        Task<string> SaveFileAsync(Stream fileStream, string fileName);
        Task DeleteFileAsync(string filePath);
    }

    public class FileStorageService : IFileStorageService
    {
        public async Task<string> SaveFileAsync(Stream fileStream, string fileName)
        {
            // Implement file saving logic.
            await Task.CompletedTask;
            return $"files/{fileName}";
        }

        public async Task DeleteFileAsync(string filePath)
        {
            // Implement file deletion logic.
            await Task.CompletedTask;
        }
    }
}
