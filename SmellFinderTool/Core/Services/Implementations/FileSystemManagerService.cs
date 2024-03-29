using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SmellFinderTool.Core.Models;
using SmellFinderTool.Core.Services.Interfaces;

namespace SmellFinderTool.Core.Services.Implementations
{
    public class FileSystemManagerService : IFileSystemManagerService
    {
        #region Methods
        public bool IsValidDirectory(string directoryName)
        {
            if (string.IsNullOrEmpty(directoryName)) return false;
            
            return Directory.Exists(directoryName);
        }

        public string FilenameOutput(string path, string extension) => 
            Path.Combine(
                path, 
                $"BS-{DateTime.Now.ToString("yyyyMMddHHmmssffff")}.{extension}"
            );

        public async Task<List<string>> FilesToProcess(string directory) {
            if (!IsValidDirectory(directory)) throw new Exception(
                string.Join(
                    " ",
                    $"{MessageFormatterModel.GetFormattedText("white on red", "\n FAIL ")}",
                    $"{MessageFormatterModel.GetFormattedText("red", "path")}",
                    $"{directory}",
                    $"{MessageFormatterModel.GetFormattedText("red", "doesn't exist")}"
                )
            );
            
            return await Task.Run(() =>
                {
                    return Directory
                        .GetFiles(directory, "*.js", SearchOption.AllDirectories)
                        .Where(x => !x.EndsWith(".min.js") && !x.EndsWith(".slim.js") && !x.EndsWith(".mjs"))
                        .ToList();
                }
            );
        }

        public string LoadFileContent(string filePath)
        {
            try
            {
                return File.ReadAllText(filePath);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}