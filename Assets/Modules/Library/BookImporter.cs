using System.Collections.Generic;
using System.IO;
using Modules.Book;
using Modules.Common;


namespace Modules.Library {
    
    public class BookImporter<T> {
        
        public T contents { get; private set; }

        public void importFromLocal(BasicBook basicBook) {
            string fileExt = FileUtils.getFileExt(basicBook.getOriginUrl());
            BookFormat bookFormat = BookFormatUtils.fromString(fileExt);
            loadContent(basicBook);
//            Library.Instance.addBook(basicBook);
        }
        
        public void importFromLocal(PdfSvgBook pdfSvgBook) {
            string dirName;
            if (pdfSvgBook.getBookMetaInfo() != null && pdfSvgBook.getBookMetaInfo().title.Length > 0) {
                dirName = pdfSvgBook.getBookMetaInfo().title.ToLower();
            }
            else {
                dirName = FileUtils.getFileNameFromPath(pdfSvgBook.getOriginUrl()).ToLower();
            }

            string outputDir = Config.Instance.getAppDir() + "/" + dirName;
            Directory.CreateDirectory(outputDir);
            PdfConversion.toSvgs(pdfSvgBook.getOriginUrl(), outputDir);

            loadContent(pdfSvgBook, outputDir);
//            Library.Instance.addBook(pdfSvgBook);
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
                    book.addPageAt(new TextPage("Page " + (pageCount + 1), pageCount + 1, pageLines), pageCount);
                    pageCount++;
                    pageLines = new string[book.linesPerPage];
                }
                pageLines[currentPageLine] = lines[i];
                currentPageLine++;
            }
        }
        
        public static void loadContent(PdfSvgBook book, string imgOutputPath) {
            int pageNb = 0;

            List<string> svgs = FileUtils.readAllSvgFiles(imgOutputPath);
            foreach(string svg in svgs) {
                SvgImagePage page = new SvgImagePage(pageNb.ToString(), pageNb, svg);
                book.appendPage(page);
                pageNb++;
            }
        }
    }
}