using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmellFinderTool.Core.Services.Interfaces
{
    public interface IFileSystemManagerService
    {
        bool IsValidDirectory(string directoryName);
        string GetFileNameOutput(string path, string extension);
        Task<List<string>> GetFilesToProcess(string directory);
        string LoadFileContent(string filePath);
    }
}