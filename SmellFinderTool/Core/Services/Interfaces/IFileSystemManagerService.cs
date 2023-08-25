using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmellFinderTool.Core.Services.Interfaces
{
    public interface IFileSystemManagerService
    {
        bool IsValidDirectory(string directoryName);
        string FilenameOutput(string path, string extension);
        Task<List<string>> FilesToProcess(string directory);
        string LoadFileContent(string filePath);
    }
}