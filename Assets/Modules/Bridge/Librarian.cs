using System;
using System.Collections.Generic;
using Modules.Common;
using Modules.EReader;

namespace Modules.Bridge {
    
    public static class Librarian {

        public static Dictionary<string, string> requestAllBookIdNamePairs() {
            Dictionary<string, Book> books = Library.Instance.books;
            Dictionary<string, string> bookIdNamePair = new Dictionary<string, string>();
            foreach(KeyValuePair<string, Book> book in books) {
                bookIdNamePair[book.Key] = book.Value.bookMetaInfo.title;
            }

            return bookIdNamePair;
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
            return getBookById(bookId).bookMetaInfo.title;
        }
        
        public static string requestAuthor(string bookId) {
            return getBookById(bookId).bookMetaInfo.author;
        }
        
        public static string requestPublisher(string bookId) {
            return getBookById(bookId).bookMetaInfo.publisher;
        }
        
        public static string requestLanguage(string bookId) {
            return getBookById(bookId).bookMetaInfo.language;
        }
        
        public static string requestDescription(string bookId) {
            return getBookById(bookId).bookMetaInfo.description;
        }
        
        public static string requestCategory(string bookId) {
            return getBookById(bookId).bookMetaInfo.category;
        }
        
        public static string[] requestTags(string bookId) {
            return getBookById(bookId).bookMetaInfo.tags;
        }
        
        public static DateTime requestPublicationDate(string bookId) {
            return getBookById(bookId).bookMetaInfo.publicationDate;
        }
        
        public static int requestPageCount(string bookId) {
            return Library.Instance.retrieveBook(bookId).getPageCount();
        }

        public static ContentType requestContentType(String bookId) {
            BookFormat bookFormat = Library.Instance.retrieveBook(bookId).getBookFormat();
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
        
        public static Object requestPageContent(string bookId, int pageNum) {
            return Library.Instance.retrieveBook(bookId).getPage(pageNum).getContent();
        }

        private static string getBookIdFromTitle(string title) {
            foreach(KeyValuePair<string, Book> entry in Library.Instance.books) {
                if (entry.Value.bookMetaInfo != null && entry.Value.bookMetaInfo.title == title) {
                    return entry.Key;
                }
                else if (FileUtils.getFileNameFromPath(entry.Value.getOriginUrl()) == title) {
                    return entry.Key;
                }
            }
            throw new BookNotFoundException("No book with title " + title + " found");
        }

        private static Book getBookByTitle(string title) {
            string bookId = getBookIdFromTitle(title);
            return getBookById(bookId);
        }

        private static Book getBookById(string bookId) {
            Book book;
            bool exists = Library.Instance.books.TryGetValue(bookId, out book);
            if (exists) {
                return book;
            }
            else {
                throw new BookNotFoundException("No book with id " + bookId + " found");
            }
        }
    }
}