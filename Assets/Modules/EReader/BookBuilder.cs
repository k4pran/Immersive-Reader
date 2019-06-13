using System.Collections.Generic;
using UnityEngine.UIElements;

namespace EReader {
    
    public class BookBuilder {

        public static Book buildBasicBook(string[] lines, int linesPerPage, BookMetaInfo bookMetaInfo) {
            BasicBook basicBook = new BasicBook(bookMetaInfo, linesPerPage, BookFormat.TEXT);

            string[] pageLines = new string[linesPerPage];
            int currentPageLine = 0;
            int pageCount = 0;
            for(int i = 0; i < lines.Length; i++) {
                
                if (currentPageLine >= linesPerPage) {
                    currentPageLine = 0;
                    basicBook.addPageAt(new BasicPage(pageLines, pageCount + 1), pageCount);
                    pageCount++;
                    pageLines = new string[linesPerPage];
                }
                pageLines[currentPageLine] = lines[i];
                currentPageLine++;
            }
            return basicBook;
        }
    }
}