using System;

namespace Modules.Library {
    
    public class BookConverter {

        public static void pdfToSvgs(Uri inputPath, Uri outputDir, string bookTitle) {
            // todo determine is pdf
            PdfConversion.toSvgs(inputPath.AbsolutePath, outputDir.AbsolutePath, bookTitle);
        }
    }
}