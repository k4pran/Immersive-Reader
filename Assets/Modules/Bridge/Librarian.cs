using System;
using EReader;
using VirtualBook;

namespace Bridge {
    
    public static class Librarian {

        public static int requestPageCount(string bookId) {
            return Library.Instance.retrieveBook(bookId).getPageCount();
        }

        public static ContentType getContentType(String bookId) {
            BookFormat bookFormat = Library.Instance.retrieveBook(bookId).getBookFormat();
            switch(bookFormat) {
                case BookFormat.TEXT:
                    return ContentType.TEXT_ONLY;
                
                case BookFormat.PDF:
                    return ContentType.IMAGE;
                
                default:
                    // todo throw exception
            }
        }
        
        public static Object requestPageContent(string bookId, int pageNum) {
            return Library.Instance.retrieveBook(bookId).getPage(pageNum).getContent();
        }
    }
}