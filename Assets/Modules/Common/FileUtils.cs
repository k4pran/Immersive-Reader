using System.Collections.Generic;
using System.IO;

namespace Modules.Common {

    public static class FileUtils {

        public static string FileExtFromPath(string path) {
            return Path.GetExtension(path);
        }

        public static string FileNameFromPath(string path, bool includeExtension = true) {
            return includeExtension ? Path.GetFileName(path) : Path.GetFileNameWithoutExtension(path);
        }

        public static List<string> ReadAllSvgFiles(string dirPath) {
            var svgs = new List<string>();
            if (Directory.Exists(dirPath)) {
                var filePaths = Directory.GetFiles(dirPath);
                foreach (var filePath in filePaths) {
                    var ext = FileExtFromPath(filePath);
                    if (ext == ".svg") svgs.Add(SvgToString(filePath));
                }

                return svgs;
            }

            throw new DirectoryNotFoundException("Unable to directory at path " + dirPath);
        }

        public static string SvgToString(string path) {
            if (File.Exists(path)) {
                var ext = FileExtFromPath(path);
                if (ext == ".svg") return File.ReadAllText(path);
                throw new FileNotFoundException("File at path " + path + " is not an svg file");
            }

            throw new FileNotFoundException("Unable to find svg file at path " + path);
        }
    }
}