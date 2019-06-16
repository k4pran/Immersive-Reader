using System;
using System.IO;
using Modules.Common;
using Modules.EReader;

namespace Modules.Bridge {
    
    public class BookImporter<T> {
        
        public T contents { get; private set; }

        public void loadFromLocal(String path, BookMetaInfo bookMetaInfo=null) {
            string fileExt = FileUtils.getFileExt(path);
            BookFormat bookFormat = BookFormatUtils.fromString(fileExt);

            switch(bookFormat) {
                    
                case BookFormat.TEXT:
                    loadDotText(path, bookMetaInfo);
                    break;
                
                case BookFormat.PDF:
                    loadPdf(path, bookMetaInfo);
                    break;
                
                default:
                    throw new BookLoadException("Failed to load book from path " + path + ".\n" +
                                                "Did not match a known book format");
            }
        }
        
        public void loadFromUrl(String url) {
            // todo
        }

        public Book loadDotText(String path, BookMetaInfo bookMetaInfo=null) {
            String[] lines = File.ReadAllLines(path);
            Book book = BookBuilder.buildBasicBook(lines, Config.Instance.linesPerPage, bookMetaInfo);
            Library.Instance.addBook(book);
            return book;
        }
        
        public void loadPdf(String path, BookMetaInfo bookMetaInfo=null) {
            PdfConversion.toJpegs(path, "");
        }

        public void addToLibrary(Book book) {
            Library.Instance.addBook(book);
        }
    }

    public class BookLoadException : Exception {

        public BookLoadException(String message) : base(message) {}

    }
}