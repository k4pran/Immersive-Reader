using System;
using JetBrains.Annotations;
using Modules.Book;

namespace Modules.Library {
    
    public interface ILibrary {

        void setup();

        IObservable<LibraryManifest> retrieveLibraryManifest();

        IObservable<LibraryManifest> addBookToLibraryManifest(BookManifest bookManifest);
        
        IObservable<LibraryManifest> saveLibraryManifest();

        IObservable<BookManifest> importBook(Uri bookPath, [CanBeNull] BookMetaInfo bookMetaInfo=null);
        
        IObservable<BookManifest> retrieveBookManifest(string bookID);

        IObservable<byte[]> retrieveBook(string bookId);
        
        IObservable<byte[]> retrieveBookMetaData(string bookId);

        IObservable<int> getBookCount();
    }
}