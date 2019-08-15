using System;
using System.Collections.Generic;
using System.IO;
using Modules.Book.Tests.Book;
using Modules.Book.Tests.Common;


namespace Modules.Library {
    
    public class BookImporter<T> {
        
        public T contents { get; private set; }

        public void importFromLocal(BasicBook basicBook) {
            string fileExt = FileUtils.getFileExt(basicBook.getOriginUrl());
            BookFormat bookFormat = BookFormatUtils.fromString(fileExt);
            loadContent(basicBook);
            Library.Instance.addBook(basicBook);
        }
        
        public void importFromLocal(PdfBasicBook pdfBasicBook) {
            string fileExt = FileUtils.getFileExt(pdfBasicBook.getOriginUrl());
            BookFormat bookFormat = BookFormatUtils.fromString(fileExt);

            string dirName;
            if (pdfBasicBook.getBookMetaInfo() != null && pdfBasicBook.getBookMetaInfo().title.Length > 0) {
                dirName = pdfBasicBook.getBookMetaInfo().title.ToLower();
            }
            else {
                dirName = FileUtils.getFileNameFromPath(pdfBasicBook.getOriginUrl()).ToLower();
            }

            string outputDir = Config.Instance.getAppDir() + "/" + dirName;
            Directory.CreateDirectory(outputDir);
            PdfConversion.toJpegs(pdfBasicBook.getOriginUrl(), outputDir);

            loadContent(pdfBasicBook, outputDir);
            Library.Instance.addBook(pdfBasicBook);
        }
        
        public static void loadContent(BasicBook book) {
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
        }
        
        public static void loadContent(PdfBasicBook book, string imgOutputPath) {
            int pageNb = 0;

            string[] filePaths = Directory.GetFiles(imgOutputPath);
            foreach(string filePath in filePaths) {
                ImagePage page = new ImagePage(filePath, pageNb, book.getOriginUrl());
                book.appendPage(page);
                pageNb++;
            }
        }
    }
}