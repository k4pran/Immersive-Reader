using System;

namespace Modules.Library {

    public class BookConverter {

        public static void PdfToSvgs(Uri inputPath, Uri outputDir, string bookTitle) {
            // todo determine is pdf
            PdfConversion.ToSvgs(inputPath.AbsolutePath, outputDir.AbsolutePath, bookTitle);
        }
    }
}