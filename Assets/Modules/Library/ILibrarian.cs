using System;
using Modules.Common;
using UnityEngine.UI;

namespace Modules.Library {

    public interface ILibrarian {

        IObservable<BookToken> AvailableBooks();

        IObservable<string> BookTitleById(string bookId);

        IObservable<string> BookIdByTitle(string bookTitle);
        
        IObservable<string> PageContents(string bookTitle);

        IObservable<ContentType> ContentType(string bookTitle);
        
        IObservable<string> PageCount(string bookTitle);

        IObservable<string> Author(string bookTitle);

        IObservable<string> Publisher(string bookTitle);

        IObservable<string> Language(string bookTitle);

        IObservable<string> Description(string bookTitle);

        IObservable<string[]> Category(string bookTitle);

        IObservable<string> Tags(string bookTitle);

        IObservable<string> PublicationDate(string bookTitle);
    }
}