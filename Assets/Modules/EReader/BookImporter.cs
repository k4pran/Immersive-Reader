using System;
using System.IO;
using Common;

namespace EReader {
    
    public class BookImporter<T> {
        
        public T contents { get; private set; }

        public void loadFromLocal(String path) {
            String fileExt = FileUtils.getFileExt(path);
            BookFormat bookFormat = BookFormatUtils.fromString(fileExt);

            switch(bookFormat) {
                    
                case BookFormat.TEXT:
                    loadText(path);
                    break;
                
                case BookFormat.PDF:
                    loadPdf(path);
                    break;
                
                default:
                    throw new BookLoadException("Failed to load book from path " + path + ".\n" +
                                                "Did not match a known book format");
            }
        }
        
        public void loadFromUrl(String url) {
            // todo
        }

        public String[] loadText(String path) {
            return File.ReadAllLines(path);
        }
        
        public void loadPdf(String path) {
            PdfConversion.toJpegs(path, "");
        }
    }

    public class BookLoadException : Exception {

        public BookLoadException(String message) : base(message) {}

    }
}