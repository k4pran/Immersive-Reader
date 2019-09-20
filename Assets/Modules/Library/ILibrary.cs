using System;
using JetBrains.Annotations;
using Modules.Book;

namespace Modules.Library {
    
    public interface ILibrary {
        
        IObservable<LibraryManifest> retrieveLibraryManifest();
        
        IObservable<BookManifest> importBook(Uri bookInputPath, [CanBeNull] BookMetaInfo bookMetaInfo=null);
        
        IObservable<BookManifest> retrieveBookManifest(string bookID);
        
        IObservable<BookMetaInfo> retrieveBookMetaInfo(string bookId);

        IObservable<int> getBookCount();
    }
}