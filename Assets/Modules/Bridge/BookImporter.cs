using System;
using System.IO;
using Modules.Common;
using Modules.EReader;

namespace Modules.Bridge {
    
    public class BookImporter<T> {
        
        public T contents { get; private set; }

        public void loadFromLocal(Book book) {
            string fileExt = FileUtils.getFileExt(book.getOriginUrl());
            BookFormat bookFormat = BookFormatUtils.fromString(fileExt);

            switch(bookFormat) {
                    
                case BookFormat.TEXT:
                    loadDotText(book);
                    break;
                
                case BookFormat.PDF:
                    loadPdf(book);
                    break;
                
                default:
                    throw new BookLoadException("Failed to load book from path " + book.getOriginUrl() + ".\n" +
                                                "Did not match a known book format");
            }
        }
        
        public void loadFromUrl(String url) {
            // todo
        }

        public Book loadDotText(Book book) {
            book = loadContent((BasicBook) book);
            addToLibrary(book);
            return book;
        }
        
        public void loadPdf(Book book) {
            string dirName;
            if (book.bookMetaInfo != null && book.bookMetaInfo.title.Length > 0) {
                dirName = book.bookMetaInfo.title.ToLower();
            }
            else {
                dirName = FileUtils.getFileNameFromPath(book.getOriginUrl()).ToLower();
            }

            string outputDir = Config.Instance.getAppDir() + "/" + dirName;
            Directory.CreateDirectory(outputDir);
            PdfConversion.toJpegs(book.getOriginUrl(), outputDir);

            loadContent((PdfBasicBook) book, outputDir);
            addToLibrary(book);
        }

        public void addToLibrary(Book book) {
            Library.Instance.addBook(book);
        }
        
        public static Book loadContent(BasicBook book) {
            string[] lines = File.ReadAllLines(book.getOriginUrl());
            string[] pageLines = new string[book.linesPerPage];
            int currentPageLine = 0;
            int pageCount = 0;
            for(int i = 0; i < lines.Length; i++) {
                
                if (currentPageLine >= book.linesPerPage) {
                    currentPageLine = 0;
                    book.addPageAt(new BasicPage(pageLines, pageCount + 1), pageCount);
                    pageCount++;
                    pageLines = new string[book.linesPerPage];
                }
                pageLines[currentPageLine] = lines[i];
                currentPageLine++;
            }
            return book;
        }
        
        public static Book loadContent(PdfBasicBook book, string imgOutputPath) {
            int pageNb = 0;

            string[] filePaths = Directory.GetFiles(imgOutputPath);
            foreach(string filePath in filePaths) {
                Page page = new ImagePage(filePath, pageNb);
                book.appendPage(page);
                pageNb++;
            }

            return book;
        }
        
    }

    public class BookLoadException : Exception {

        public BookLoadException(String message) : base(message) {}

    }
}