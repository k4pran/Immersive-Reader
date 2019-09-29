using System;
using Modules.Common;

namespace Modules.Library {

    public interface ILibrarian {

        IObservable<BookToken> AvailableBooks();

        IObservable<string> BookTitleById(string bookId);

        IObservable<string> BookIdByTitle(string bookTitle);
        
        IObservable<int> BookCount();

        IObservable<ContentType> BookType(string bookId);

        IObservable<T> PageContents<T>(string bookId, int pageNb);
        
        IObservable<int> PageCount(string bookId);

        IObservable<string> Author(string bookId);

        IObservable<string> Publisher(string bookId);

        IObservable<string> Language(string bookId);

        IObservable<string> Description(string bookId);

        IObservable<string> Category(string bookId);

        IObservable<string[]> Tags(string bookId);

        IObservable<DateTime> PublicationDate(string bookId);
    }
}