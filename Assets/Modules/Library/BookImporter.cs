using System;
using System.Collections.Generic;
using System.IO;
using Modules.Common;
using Modules.Book;


namespace Modules.Bridge {
    
    public class BookImporter<T> {
        
        public T contents { get; private set; }

        public void loadFromLocal(Book<object> book) {
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

        public void loadDotText(Book<object> book) {
            BasicBook basicBook = new BasicBook(book, Config.Instance.linesPerPage);
            loadContent(basicBook);
            Library.Instance.addBook(basicBook);
        }
        
        public void loadPdf(Book<object> book) {
            string dirName;
            if (book.getBookMetaInfo() != null && book.getBookMetaInfo().title.Length > 0) {
                dirName = book.getBookMetaInfo().title.ToLower();
            }
            else {
                dirName = FileUtils.getFileNameFromPath(book.getOriginUrl()).ToLower();
            }

            string outputDir = Config.Instance.getAppDir() + "/" + dirName;
            Directory.CreateDirectory(outputDir);
            PdfConversion.toJpegs(book.getOriginUrl(), outputDir);

            PdfBasicBook pdfBasicBook = new PdfBasicBook(book);
            loadContent(pdfBasicBook, outputDir);
            Library.Instance.addBook(pdfBasicBook);
        }

        public static BasicBook loadContent(BasicBook book) {
            string[] lines = File.ReadAllLines(book.getOriginUrl());
            string[] pageLines = new string[book.linesPerPage];
            new List<string>(lines);
            int currentPageLine = 0;
            int pageCount = 0;
            for(int i = 0; i < lines.Length; i++) {
                
                if (currentPageLine >= book.linesPerPage) {
                    currentPageLine = 0;
                    book.addPageAt(new TextPage("Page " + (pageCount + 1), pageCount + 1, lines), pageCount);
                    pageCount++;
                    pageLines = new string[book.linesPerPage];
                }
                pageLines[currentPageLine] = lines[i];
                currentPageLine++;
            }
            return book;
        }
        
        public static PdfBasicBook loadContent(PdfBasicBook book, string imgOutputPath) {
            int pageNb = 0;

            string[] filePaths = Directory.GetFiles(imgOutputPath);
            foreach(string filePath in filePaths) {
                ImagePage page = new ImagePage(filePath, pageNb, book.getOriginUrl());
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