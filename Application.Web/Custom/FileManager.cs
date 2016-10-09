using System;
using System.IO;
using System.Linq;

namespace Application.Web.Custom
{
    public enum FileExtension
    {
        Xls,
        Xlsx
    }

    public class FileManager
    {
        private readonly string _genPath;
        private string _fileName;
        private const string FilePrefix = "DozTendersExp_";

        public FileManager()
        {            
            _genPath = Path.GetTempPath();
        }

        public void DeleteOldFiles(int min)
        {
            var invalidFileTime = DateTime.Now.AddMinutes(min * -1);
            var invalidFiles = new DirectoryInfo(_genPath)
                        .GetFiles()
                        .Where(f => f.Name.StartsWith(FilePrefix) &&
                                    f.CreationTime < invalidFileTime)
                        .Select(f => f.FullName)
                        .ToList();            
            invalidFiles.ForEach(File.Delete);
        }

        public string GenerateFileName(FileExtension extension)
        {
            _fileName = FilePrefix + Guid.NewGuid().ToString("N") + "." + extension;
            return _fileName;
        }

        public string GetFullFilename()
        {
            return _genPath + _fileName;
        }
    }
}