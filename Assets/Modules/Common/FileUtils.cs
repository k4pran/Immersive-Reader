using System;
using System.Collections.Generic;
using System.IO;

namespace Modules.Common {
    
    public static class FileUtils {

        public static string getFileExt(string path) {
            return Path.GetExtension(path);
        }

        public static string getFileNameFromPath(string path) {
            return Path.GetFileNameWithoutExtension(path);
        }

        public static List<string> readAllSvgFiles(string dirPath) {
            List<string> svgs = new List<string>();
            if (Directory.Exists(dirPath)) {
                string[] filePaths = Directory.GetFiles(dirPath);
                foreach (string filePath in filePaths) {
                    string ext = getFileExt(filePath);
                    if (ext == ".svg") {
                        svgs.Add(svgToString(filePath));
                    }
                }
                return svgs;
            }
            throw new DirectoryNotFoundException("Unable to directory at path " + dirPath);
        }

        public static string svgToString(string path) {
            if (File.Exists(path)) {
                string ext = getFileExt(path);
                if (ext == ".svg") {
                    return File.ReadAllText(path);
                }
                throw new FileNotFoundException("File at path " + path + " is not an svg file");
            }
            throw new FileNotFoundException("Unable to find svg file at path " + path);
        }
    }
}
