using System;

namespace Modules.Library {

    public interface ILibrarian {

        IObservable<BookToken> AvailableBooks();

        IObservable<string> BookTitleById(string bookId);

        IObservable<string> BookIdByTitle(string bookTitle);
    }
}