using System;

namespace Modules.Library {
    
    public class Indexer {

        public static void asSvgs(Uri inputPath, Uri outputDir, string bookTitle) {
            // todo determine is pdf
            PdfConversion.toSvgs(inputPath.AbsolutePath, outputDir.AbsolutePath, bookTitle);
        }
    }
}