using System.Diagnostics;
using System.IO;
using Modules.Common;
using Debug = UnityEngine.Debug;

namespace Modules.EReader {
    public class PdfConversion {

        private static readonly string BINARY_DIR = "Assets/Modules/EReader/bin";
        private static readonly string JPEG_EXECUTABLE_NAME = "pdftocairo";
        private static readonly string SVG_EXECUTABLE_NAME = "pdf2svg";

        public static void toJpegs(string inputPath, string outputPath) {
            string absBinPath = resolvePath(Path.Combine(BINARY_DIR, JPEG_EXECUTABLE_NAME));
            // Handle pdfcairo quirk by adding dir name twice - one for directory and second for file naming
            string absOutPath = resolvePath(outputPath) + "/" + FileUtils.getFileNameFromPath(outputPath);
            convert(absBinPath, inputPath, "-jpeg '" + inputPath + "' '" + absOutPath + "'");
        }

        public static void toSvgs(string inputPath, string outputPath) {
            string absPath = resolvePath(Path.Combine(BINARY_DIR, SVG_EXECUTABLE_NAME));
            string absOutPath = resolvePath(outputPath);
            convert(absPath, inputPath, "'" + inputPath + "' '" + absOutPath + "-%d.svg' all");
        }

        private static void convert(string executable, string inputPath, string arguments) {
            
            if (File.Exists(inputPath)){
                var p = new Process();
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.FileName = executable;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.Arguments = arguments;


                p.Start();

                Debug.Log(p.StandardError.ReadToEnd()); 
                Debug.Log(p.StandardOutput.ReadToEnd());

                p.WaitForExit(); 
            }
            
            else {
                Debug.Log("File " + inputPath + " does not exist");
            }
        }

        private static string resolvePath(string relPath) {
            return Path.GetFullPath(relPath);
        }
    }
}