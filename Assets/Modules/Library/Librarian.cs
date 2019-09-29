using System;
using System.Reactive.Linq;
using Modules.Book;
using Modules.Common;

namespace Modules.Library {

    public class Librarian : ILibrarian {

        private IBook<Page> book;

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

        public IObservable<ContentType> BookType(string bookId) {
            return library.RetrieveBookManifest(bookId)
                .Select(bookManifest => bookManifest.contentType);
        }

        public IBook<Page> PageContents(string bookId) {
            return new BasicBook(null, Binding.DOUBLE_PAGED, null);
        }
    }
}