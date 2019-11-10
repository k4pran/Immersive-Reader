using System;
using Modules.Common;

namespace Modules.Library {

    public class BookConverter {

        public static void PdfToSvgs(Uri inputPath, Uri outputDir, string bookTitle) {
            // todo determine is pdf
            Logger.Debug($"Converting pdf at {inputPath} to svgs at target destination directory {outputDir} " +
                         $"for book {bookTitle}");
            PdfConversion.ToSvgs(inputPath.AbsolutePath, outputDir.AbsolutePath, bookTitle);
        }
    }
}