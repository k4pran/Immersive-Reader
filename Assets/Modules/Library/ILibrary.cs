using System;
using System.Collections.Generic;
using Modules.Book;
using Modules.Common;

namespace Modules.Library {

    public interface ILibrary {

        IObservable<LibraryManifest> RetrieveLibraryManifest();

        IObservable<BookManifest> ImportBook(Uri bookInputPath, BookMetaInfo bookMetaInfo, ContentType contentType,
            params KeyValuePair<Option, object>[] options);

        IObservable<BookManifest> RetrieveBookManifest(string bookId);

        IObservable<BookMetaInfo> RetrieveBookMetaInfo(string bookId);

        IObservable<Uri> RetrievePhysicalFileLocation(string bookId);

        IObservable<IBook<Page>> ReadBookAsObject(string bookId);

        IObservable<string> ReadBookAsString(string bookId);

        IObservable<string[]> ReadBookAsLines(string bookId);

        IObservable<byte[]> ReadBookAsBytes(string bookId);
        
        IObservable<T> ReadPage<T>(string bookId, int pageNb);

        IObservable<string> ReadPageAsString(string bookId, int pageNb);

        IObservable<string[]> ReadPageAsLines(string bookId, int pageNb);

        IObservable<byte[]> ReadPageAsBytes(string bookId, int pageNb);
        
        IObservable<int> GetBookPageCount(string bookId);

        IObservable<int> GetBookCount();
    }
}