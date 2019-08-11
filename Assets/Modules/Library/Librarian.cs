using System;
using System.Collections.Generic;
using Modules.Common;
using Modules.Book;

namespace Modules.Bridge {
    
    public static class Librarian {

        public static Dictionary<string, string> requestAllBookIdNamePairs() {
            throw new NotImplementedException();
        }

        public static bool doesBookIdExist(string bookId) {
            return Library.Instance.doesLibraryContainId(bookId);
        }
        
        public static bool doesBookTitleExist(string title) {
            return Library.Instance.doesLibraryContainTitle(title);
        }

        public static string requestId(string title) {
            return getBookIdFromTitle(title);
        }

        public static string requestTitle(string bookId) {
            return getBookById(bookId).getBookMetaInfo().title;
        }
        
        public static string requestAuthor(string bookId) {
            return getBookById(bookId).getBookMetaInfo().author;
        }
        
        public static string requestPublisher(string bookId) {
            return getBookById(bookId).getBookMetaInfo().publisher;
        }
        
        public static string requestLanguage(string bookId) {
            return getBookById(bookId).getBookMetaInfo().language;
        }
        
        public static string requestDescription(string bookId) {
            return getBookById(bookId).getBookMetaInfo().description;
        }
        
        public static string requestCategory(string bookId) {
            return getBookById(bookId).getBookMetaInfo().category;
        }
        
        public static string[] requestTags(string bookId) {
            return getBookById(bookId).getBookMetaInfo().tags;
        }
        
        public static DateTime requestPublicationDate(string bookId) {
            return getBookById(bookId).getBookMetaInfo().publicationDate;
        }
        
        public static int requestPageCount(string bookId) {
            throw new NotImplementedException();
        }

        public static ContentType requestContentType<T>(String bookId) {
            BookFormat bookFormat = Library.Instance.retrieveBook<T>(bookId).getBookFormat();
            switch(bookFormat) {
                case BookFormat.TEXT:
                    return ContentType.TEXT_ONLY;
                
                case BookFormat.PDF:
                    return ContentType.IMAGE;
                
                default:
                    throw new ContentTypeException("Unable to resolve content type. It may be the case that" +
                                                   "this content is not yet supported.");
            }
        }
        
        public static Book<T> requestPageContent<T>(string bookId, int pageNum) {
            throw new NotImplementedException();
        }

        private static string getBookIdFromTitle(string title) {
            throw new NotImplementedException();

        }

        private static Book<object> getBookByTitle(string title) {
            string bookId = getBookIdFromTitle(title);
            return getBookById(bookId);
        }

        private static Book<object> getBookById(string bookId) {
            throw new NotImplementedException();
            throw new BookNotFoundException("No book with id " + bookId + " found");
        }
    }
}