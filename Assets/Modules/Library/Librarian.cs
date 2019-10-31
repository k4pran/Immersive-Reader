using System;
using System.Reactive.Linq;
using Modules.Common;

namespace Modules.Library {

    public class Librarian : ILibrarian {
        
        private readonly ILibrary library;

        public Librarian(ILibrary library) {
            this.library = library;
        }

        public IObservable<BookToken> AvailableBooks() {
            return library.RetrieveLibraryManifest()
                .Do(_ => Logger.Debug("Fetching available books from library"))
                .SelectMany(libraryManifest => libraryManifest.GetBookTokens())
                .Do(token => Logger.Debug($"Book found [{token.bookTitle} : {token.bookId}]"));
        }

        public IObservable<string> BookTitleById(string bookId) {
            return library.RetrieveLibraryManifest()
                .Do(_ => Logger.Debug($"Fetching title for book {bookId}"))
                .SelectMany(libraryManifest => libraryManifest.GetBookTokens())
                .Where(token => token.bookId.Equals(bookId))
                .Select(token => token.bookTitle)
                .Do(bookTitle => Logger.Debug($"Found match {bookTitle} for book {bookId}"))
                .SingleAsync();
        }

        public IObservable<string> BookIdByTitle(string bookTitle) {
            return library.RetrieveLibraryManifest()
                .Do(_ => Logger.Debug($"Fetching id for book {bookTitle}"))
                .SelectMany(libraryManifest => libraryManifest.GetBookTokens())
                .Where(token => token.bookTitle.Equals(bookTitle))
                .Select(token => token.bookId)
                .Do(bookId => Logger.Debug($"Found match {bookId} for book {bookTitle}"))
                .SingleAsync();
        }

        public IObservable<int> BookCount() {
            return library.GetBookCount()
                .Do(bookCount => Logger.Debug($"Book count found {bookCount} book(s)"));
        }

        public IObservable<ContentType> BookType(string bookId) {
            return library.RetrieveBookManifest(bookId)
                .Do(_ => Logger.Debug($"Determining book type [{bookId}]"))
                .Select(bookManifest => bookManifest.contentType)
                .Do(contentType => Logger.Debug($"Recognised book type as {contentType.ToString()}"));
        }

        public IObservable<T> PageContents<T>(string bookId, int pageNb) {
            return library.ReadBookAsObject(bookId)
                .Do(_ => Logger.Debug($"Fetching page {pageNb} of book {bookId}"))
                .Select(book => book.Page(pageNb))
                .Select(page => page.Content<T>());
        }

        public IObservable<int> PageCount(string bookId) {
            return library.ReadBookAsObject(bookId)
                .Do(_ => Logger.Trace($"Counting pages in book {bookId}"))
                .Select(book => book.PageCount())
                .Do(pageCount => Logger.Trace($"Found {pageCount} pages in book {bookId}"));
        }

        public IObservable<string> Title(string bookId) {
            return library.RetrieveBookMetaInfo(bookId)
                .Do(_ => Logger.Trace($"Fetching title for book {bookId}"))
                .Select(book => book.title)
                .Do(title => Logger.Trace($"Book title determined as {title} for {bookId}"));
        }

        public IObservable<string> Author(string bookId) {
            return library.RetrieveBookMetaInfo(bookId)
                .Do(_ => Logger.Trace($"Fetching author for book {bookId}"))
                .Select(metaInfo => metaInfo.author)
                .Do(author => Logger.Trace($"Book title determined as {author} for {bookId}"));
        }

        public IObservable<string> Publisher(string bookId) {
            return library.RetrieveBookMetaInfo(bookId)
                .Do(_ => Logger.Trace($"Fetching publisher for book {bookId}"))
                .Select(metaInfo => metaInfo.publisher)
                .Do(publisher => Logger.Trace($"Book publisher determined as {publisher} for {bookId}"));
        }

        public IObservable<string> Language(string bookId) {
            return library.RetrieveBookMetaInfo(bookId)
                .Do(_ => Logger.Trace($"Fetching language for book {bookId}"))
                .Select(metaInfo => metaInfo.language)
                .Do(language => Logger.Trace($"Book language determined as {language} for {bookId}"));
        }

        public IObservable<string> Description(string bookId) {
            return library.RetrieveBookMetaInfo(bookId)
                .Do(_ => Logger.Trace($"Fetching description for book {bookId}"))
                .Select(metaInfo => metaInfo.description);
            
        }

        public IObservable<string> Category(string bookId) {
            return library.RetrieveBookMetaInfo(bookId)
                .Do(_ => Logger.Trace($"Fetching categories for book {bookId}"))
                .Select(metaInfo => metaInfo.category);        
        }

        public IObservable<string[]> Tags(string bookId) {
            return library.RetrieveBookMetaInfo(bookId)
                .Do(_ => Logger.Trace($"Fetching tags for book {bookId}"))
                .Select(metaInfo => metaInfo.tags);       
        }

        public IObservable<DateTime> PublicationDate(string bookId) {
            return library.RetrieveBookMetaInfo(bookId)
                .Do(_ => Logger.Trace($"Fetching publication date for book {bookId}"))
                .Select(metaInfo => metaInfo.publicationDate)
                .Do(publicationDate => 
                    Logger.Trace($"Book publication date determined as {publicationDate} for {bookId}"));
        }
    }
}