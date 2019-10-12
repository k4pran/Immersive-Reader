using System.Diagnostics;
using System.IO;
using Modules.Common;
using Debug = UnityEngine.Debug;

namespace Modules.Library {

    public class PdfConversion {

        private static readonly string BINARY_DIR = "Assets/bin";
        private static readonly string JPEG_EXECUTABLE_NAME = "pdftocairo";
        private static readonly string SVG_EXECUTABLE_NAME = "pdf2svg";

        public static void ToJpegs(string inputPath, string outputPath) {
            var absBinPath = ResolvePath(Path.Combine(BINARY_DIR, JPEG_EXECUTABLE_NAME));
            // Handle pdfcairo quirk by adding dir name twice - one for directory and second for file naming
            var absOutPath = ResolvePath(outputPath) + "/" + FileUtils.FileNameFromPath(outputPath);
            Convert(absBinPath, inputPath, "-jpeg '" + inputPath + "' '" + absOutPath + "'");
        }

        public static void ToSvgs(string inputPath, string outputDir, string bookTitle) {
            var absPath = ResolvePath(Path.Combine(BINARY_DIR, SVG_EXECUTABLE_NAME));
            Convert(absPath, inputPath, "'" + inputPath + "' '" + outputDir + "/" + bookTitle + "-%d.svg' all");
        }

        private static void Convert(string executable, string inputPath, string arguments) {
            if (File.Exists(inputPath)) {
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

        private static string ResolvePath(string relPath) {
            return Path.GetFullPath(relPath);
        }
    }
}