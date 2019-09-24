using System;
using Modules.Book;
using Modules.Common;
using NUnit.Framework;
using UnityEngine;

namespace Modules.Library.Tests {
    
    public class VfsLibraryTests {
        
        [Test]
        public void importBook() {
            BookMetaInfo bookMetaInfo = new BookMetaInfo();
            bookMetaInfo.title = "Dracula";
            bookMetaInfo.author = "Bram Stoker";
            bookMetaInfo.publisher = "Archibald Constable and Company (UK)";
            bookMetaInfo.pageCount = 368;
            bookMetaInfo.language = "English";
            bookMetaInfo.description = "Dracula is an 1897 Gothic horror novel by Irish author Bram Stoker";
            bookMetaInfo.publicationDate = new DateTime(1987, 5, 26);
            bookMetaInfo.category = "Gothic horror";
            bookMetaInfo.tags = new[] {"gothic", "horror", "vampires", "classic"};
            
            VirtualFileLibrary virtualFileLibrary = new VirtualFileLibrary();
            Uri uri = new Uri(
                "file:///Users/ryan/Documents/Unity/VReader_2/Assets/Modules/Book/Tests/Resources/dracula.txt");
            virtualFileLibrary.importBook(uri, bookMetaInfo).Subscribe(bookManifest => {
                    Assert.IsNotEmpty(bookManifest.bookId);
                    Assert.IsNotEmpty(bookManifest.bookLocation);
                    Assert.AreEqual(bookMetaInfo.title, bookManifest.bookTitle);
                    Assert.IsNotEmpty(bookManifest.metaInfoLocation);
                    
                },
                error => Debug.Log(error));
        }
        
        [Test]
        public void indexImportedBook() {
            BookMetaInfo bookMetaInfo = new BookMetaInfo();
            bookMetaInfo.title = "Atari";
            bookMetaInfo.author = "Deepmind";
            bookMetaInfo.publisher = "Deepmind";
            bookMetaInfo.pageCount = 9;
            bookMetaInfo.language = "English";
            bookMetaInfo.description = "A journal on reinforcement learning using atari games";
            bookMetaInfo.publicationDate = new DateTime(2013, 12, 19);
            bookMetaInfo.category = "Gothic horror";
            bookMetaInfo.tags = new[] {"journal", "reinforcement learning", "AI"};
            
            VirtualFileLibrary virtualFileLibrary = new VirtualFileLibrary();
            Uri uri = new Uri(
                "file:///Users/ryan/Documents/Unity/VReader_2/Assets/Modules/Book/Tests/Resources/atari.pdf");
            virtualFileLibrary.importBook(uri, bookMetaInfo).Subscribe(bookManifest => {
                    virtualFileLibrary.indexBook(bookManifest.bookId, ContentType.SVG);
                },
                error => Debug.Log(error));
        }

        [Test]
        public void bookCount() {
            BookMetaInfo bookMetaInfo = new BookMetaInfo();
            bookMetaInfo.title = "Dracula";
            bookMetaInfo.author = "Bram Stoker";
            bookMetaInfo.publisher = "Archibald Constable and Company (UK)";
            bookMetaInfo.pageCount = 368;
            bookMetaInfo.language = "English";
            bookMetaInfo.description = "Dracula is an 1897 Gothic horror novel by Irish author Bram Stoker";
            bookMetaInfo.publicationDate = new DateTime(1987, 5, 26);
            bookMetaInfo.category = "Gothic horror";
            bookMetaInfo.tags = new[] {"gothic", "horror", "vampires", "classic"};
            
            VirtualFileLibrary virtualFileLibrary = new VirtualFileLibrary();

            Uri uri = new Uri(
                "file:///Users/ryan/Documents/Unity/VReader_2/Assets/Modules/Book/Tests/Resources/dracula.txt");
            virtualFileLibrary.importBook(uri, bookMetaInfo).Subscribe(_ => {
                    Debug.Log(_);
                },
                error => Debug.Log(error),
                () => Assert.AreEqual(virtualFileLibrary.getBookCount(), 1));
        }
        
        [Test]
        public void retrieveLibraryManifest() {
            BookMetaInfo bookMetaInfo = new BookMetaInfo();
            bookMetaInfo.title = "Dracula";
            bookMetaInfo.author = "Bram Stoker";
            bookMetaInfo.publisher = "Archibald Constable and Company (UK)";
            bookMetaInfo.pageCount = 368;
            bookMetaInfo.language = "English";
            bookMetaInfo.description = "Dracula is an 1897 Gothic horror novel by Irish author Bram Stoker";
            bookMetaInfo.publicationDate = new DateTime(1987, 5, 26);
            bookMetaInfo.category = "Gothic horror";
            bookMetaInfo.tags = new[] {"gothic", "horror", "vampires", "classic"};
            
            VirtualFileLibrary virtualFileLibrary = new VirtualFileLibrary();

            Uri uri = new Uri(
                "file:///Users/ryan/Documents/Unity/VReader_2/Assets/Modules/Book/Tests/Resources/dracula.txt");
            virtualFileLibrary.importBook(uri, bookMetaInfo).Subscribe(bookManifest => {
                    virtualFileLibrary.retrieveLibraryManifest().Subscribe(libraryManifest => {
                        Assert.Equals(libraryManifest.bookManifests.Count, 1);
                    });
                },
                error => Debug.Log(error));
        }
        
        [Test]
        public void retrieveBookManifest() {
            BookMetaInfo bookMetaInfo = new BookMetaInfo();
            bookMetaInfo.title = "Dracula";
            bookMetaInfo.author = "Bram Stoker";
            bookMetaInfo.publisher = "Archibald Constable and Company (UK)";
            bookMetaInfo.pageCount = 368;
            bookMetaInfo.language = "English";
            bookMetaInfo.description = "Dracula is an 1897 Gothic horror novel by Irish author Bram Stoker";
            bookMetaInfo.publicationDate = new DateTime(1987, 5, 26);
            bookMetaInfo.category = "Gothic horror";
            bookMetaInfo.tags = new[] {"gothic", "horror", "vampires", "classic"};
            
            VirtualFileLibrary virtualFileLibrary = new VirtualFileLibrary();

            Uri uri = new Uri(
                "file:///Users/ryan/Documents/Unity/VReader_2/Assets/Modules/Book/Tests/Resources/dracula.txt");
            virtualFileLibrary.importBook(uri, bookMetaInfo).Subscribe(bookManifest => {
                    virtualFileLibrary.retrieveBookManifest(bookManifest.bookId).Subscribe(retrievedBookManifest => {
                        Assert.Equals(bookManifest.bookTitle, retrievedBookManifest.bookTitle);
                    });
                },
                error => Debug.Log(error));
        }
        
        [Test]
        public void retrieveBookMetaData() {
            BookMetaInfo bookMetaInfo = new BookMetaInfo();
            bookMetaInfo.title = "Dracula";
            bookMetaInfo.author = "Bram Stoker";
            bookMetaInfo.publisher = "Archibald Constable and Company (UK)";
            bookMetaInfo.pageCount = 368;
            bookMetaInfo.language = "English";
            bookMetaInfo.description = "Dracula is an 1897 Gothic horror novel by Irish author Bram Stoker";
            bookMetaInfo.publicationDate = new DateTime(1987, 5, 26);
            bookMetaInfo.category = "Gothic horror";
            bookMetaInfo.tags = new[] {"gothic", "horror", "vampires", "classic"};
            
            VirtualFileLibrary virtualFileLibrary = new VirtualFileLibrary();

            Uri uri = new Uri(
                "file:///Users/ryan/Documents/Unity/VReader_2/Assets/Modules/Book/Tests/Resources/dracula.txt");
            virtualFileLibrary.importBook(uri, bookMetaInfo).Subscribe(bookManifest => {
                    virtualFileLibrary.retrieveBookMetaInfo(bookManifest.bookId).Subscribe(retrievedBookMetaInfo => {
                        Assert.Equals(bookMetaInfo.title, retrievedBookMetaInfo.title);
                    });
                },
                error => Debug.Log(error));
        }
    }
}