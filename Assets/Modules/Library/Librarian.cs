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
                .SelectMany(libraryManifest => libraryManifest.GetBookTokens());
        }

        public IObservable<string> BookTitleById(string bookId) {
            return library.RetrieveLibraryManifest()
                .SelectMany(libraryManifest => libraryManifest.GetBookTokens())
                .Where(token => token.bookId.Equals(bookId))
                .Select(token => token.bookTitle)
                .SingleAsync();
        }

        public IObservable<string> BookIdByTitle(string bookTitle) {
            return library.RetrieveLibraryManifest()
                .SelectMany(libraryManifest => libraryManifest.GetBookTokens())
                .Where(token => token.bookTitle.Equals(bookTitle))
                .Select(token => token.bookId)
                .SingleAsync();
        }

        public IObservable<int> BookCount() {
            return library.GetBookCount();
        }

        public IObservable<ContentType> BookType(string bookId) {
            return library.RetrieveBookManifest(bookId)
                .Select(bookManifest => bookManifest.contentType);
        }

        public IObservable<T> PageContents<T>(string bookId, int pageNb) {
            return library.ReadBookAsObject(bookId)
                .Select(book => book.Page(pageNb))
                .Select(page => page.Content<T>());
        }

        public IObservable<int> PageCount(string bookId) {
            return library.ReadBookAsObject(bookId)
                .Select(book => book.PageCount());
        }

        public IObservable<string> Title(string bookId) {
            return library.RetrieveBookMetaInfo(bookId)
                .Select(book => book.title);
        }

        public IObservable<string> Author(string bookId) {
            return library.RetrieveBookMetaInfo(bookId)
                .Select(metaInfo => metaInfo.author);
        }

        public IObservable<string> Publisher(string bookId) {
            return library.RetrieveBookMetaInfo(bookId)
                .Select(metaInfo => metaInfo.publisher);        }

        public IObservable<string> Language(string bookId) {
            return library.RetrieveBookMetaInfo(bookId)
                .Select(metaInfo => metaInfo.language);        }

        public IObservable<string> Description(string bookId) {
            return library.RetrieveBookMetaInfo(bookId)
                .Select(metaInfo => metaInfo.description);        }

        public IObservable<string> Category(string bookId) {
            return library.RetrieveBookMetaInfo(bookId)
                .Select(metaInfo => metaInfo.category);        }

        public IObservable<string[]> Tags(string bookId) {
            return library.RetrieveBookMetaInfo(bookId)
                .Select(metaInfo => metaInfo.tags);        }

        public IObservable<DateTime> PublicationDate(string bookId) {
            return library.RetrieveBookMetaInfo(bookId)
                .Select(metaInfo => metaInfo.publicationDate);
        }

    }
}