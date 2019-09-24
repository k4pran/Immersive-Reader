using System;
using System.IO;
using JetBrains.Annotations;
using Modules.Book;

namespace Modules.Library {
    
    public interface ILibrary {
        
        IObservable<LibraryManifest> retrieveLibraryManifest();
        
        IObservable<BookManifest> importBook(Uri bookInputPath, BookMetaInfo bookMetaInfo=null);
        
        IObservable<BookManifest> retrieveBookManifest(string bookID);
        
        IObservable<BookMetaInfo> retrieveBookMetaInfo(string bookId);

        IObservable<Uri> retrievePhysicalFileLocation(string bookId);

        IObservable<string> readBookAsString(string bookId);
        
        IObservable<string[]> readBookAsLines(string bookId);
        
        IObservable<byte[]> readBookAsBytes(string bookId);

        IObservable<int> getBookCount();
    }
}