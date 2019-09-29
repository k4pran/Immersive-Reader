using System;
using Modules.Book;
using Modules.Common;
using NUnit.Framework;
using UnityEngine;

namespace Modules.Library.Tests {

    public class VfsLibraryTests {

        [Test]
        public void ImportBook() {
            var bookMetaInfo = new BookMetaInfo();
            bookMetaInfo.title = "Dracula";
            bookMetaInfo.author = "Bram Stoker";
            bookMetaInfo.publisher = "Archibald Constable and Company (UK)";
            bookMetaInfo.language = "English";
            bookMetaInfo.description = "Dracula is an 1897 Gothic horror novel by Irish author Bram Stoker";
            bookMetaInfo.publicationDate = new DateTime(1987, 5, 26);
            bookMetaInfo.category = "Gothic horror";
            bookMetaInfo.tags = new[] {"gothic", "horror", "vampires", "classic"};

            var virtualFileLibrary = new VirtualFileLibrary();
            var uri = new Uri(
                "file:///Users/ryan/Documents/Unity/VReader_2/Assets/Modules/Book/Tests/Resources/dracula.txt");
            virtualFileLibrary.ImportBook(uri, bookMetaInfo, ContentType.TEXT_ONLY).Subscribe(bookManifest => {
                    Assert.IsNotEmpty(bookManifest.bookId);
                    Assert.IsNotEmpty(bookManifest.bookLocation);
                    Assert.AreEqual(bookMetaInfo.title, bookManifest.bookTitle);
                    Assert.IsNotEmpty(bookManifest.metaInfoLocation);
                },
                error => Debug.Log(error));
        }

        [Test]
        public void BookCount() {
            var bookMetaInfo = new BookMetaInfo();
            bookMetaInfo.title = "Dracula";
            bookMetaInfo.author = "Bram Stoker";
            bookMetaInfo.publisher = "Archibald Constable and Company (UK)";
            bookMetaInfo.language = "English";
            bookMetaInfo.description = "Dracula is an 1897 Gothic horror novel by Irish author Bram Stoker";
            bookMetaInfo.publicationDate = new DateTime(1987, 5, 26);
            bookMetaInfo.category = "Gothic horror";
            bookMetaInfo.tags = new[] {"gothic", "horror", "vampires", "classic"};

            var virtualFileLibrary = new VirtualFileLibrary();

            var uri = new Uri(
                "file:///Users/ryan/Documents/Unity/VReader_2/Assets/Modules/Book/Tests/Resources/dracula.txt");
            virtualFileLibrary.ImportBook(uri, bookMetaInfo, ContentType.TEXT_ONLY).Subscribe(_ => { Debug.Log(_); },
                error => Debug.Log(error),
                () => Assert.AreEqual(virtualFileLibrary.GetBookCount(), 1));
        }

        [Test]
        public void RetrieveLibraryManifest() {
            var bookMetaInfo = new BookMetaInfo();
            bookMetaInfo.title = "Dracula";
            bookMetaInfo.author = "Bram Stoker";
            bookMetaInfo.publisher = "Archibald Constable and Company (UK)";
            bookMetaInfo.language = "English";
            bookMetaInfo.description = "Dracula is an 1897 Gothic horror novel by Irish author Bram Stoker";
            bookMetaInfo.publicationDate = new DateTime(1987, 5, 26);
            bookMetaInfo.category = "Gothic horror";
            bookMetaInfo.tags = new[] {"gothic", "horror", "vampires", "classic"};

            var virtualFileLibrary = new VirtualFileLibrary();

            var uri = new Uri(
                "file:///Users/ryan/Documents/Unity/VReader_2/Assets/Modules/Book/Tests/Resources/dracula.txt");
            virtualFileLibrary.ImportBook(uri, bookMetaInfo, ContentType.TEXT_ONLY).Subscribe(
                bookManifest => {
                    virtualFileLibrary.RetrieveLibraryManifest().Subscribe(libraryManifest => {
                        Assert.Equals(libraryManifest.bookManifests.Count, 1);
                    });
                },
                error => Debug.Log(error));
        }

        [Test]
        public void RetrieveBookManifest() {
            var bookMetaInfo = new BookMetaInfo();
            bookMetaInfo.title = "Dracula";
            bookMetaInfo.author = "Bram Stoker";
            bookMetaInfo.publisher = "Archibald Constable and Company (UK)";
            bookMetaInfo.language = "English";
            bookMetaInfo.description = "Dracula is an 1897 Gothic horror novel by Irish author Bram Stoker";
            bookMetaInfo.publicationDate = new DateTime(1987, 5, 26);
            bookMetaInfo.category = "Gothic horror";
            bookMetaInfo.tags = new[] {"gothic", "horror", "vampires", "classic"};

            var virtualFileLibrary = new VirtualFileLibrary();

            var uri = new Uri(
                "file:///Users/ryan/Documents/Unity/VReader_2/Assets/Modules/Book/Tests/Resources/dracula.txt");
            virtualFileLibrary.ImportBook(uri, bookMetaInfo, ContentType.TEXT_ONLY).Subscribe(
                bookManifest => {
                    virtualFileLibrary.RetrieveBookManifest(bookManifest.bookId).Subscribe(retrievedBookManifest => {
                        Assert.Equals(bookManifest.bookTitle, retrievedBookManifest.bookTitle);
                    });
                },
                error => Debug.Log(error));
        }

        [Test]
        public void RetrieveBookMetaData() {
            var bookMetaInfo = new BookMetaInfo();
            bookMetaInfo.title = "Dracula";
            bookMetaInfo.author = "Bram Stoker";
            bookMetaInfo.publisher = "Archibald Constable and Company (UK)";
            bookMetaInfo.language = "English";
            bookMetaInfo.description = "Dracula is an 1897 Gothic horror novel by Irish author Bram Stoker";
            bookMetaInfo.publicationDate = new DateTime(1987, 5, 26);
            bookMetaInfo.category = "Gothic horror";
            bookMetaInfo.tags = new[] {"gothic", "horror", "vampires", "classic"};

            var virtualFileLibrary = new VirtualFileLibrary();

            var uri = new Uri(
                "file:///Users/ryan/Documents/Unity/VReader_2/Assets/Modules/Book/Tests/Resources/dracula.txt");
            virtualFileLibrary.ImportBook(uri, bookMetaInfo, ContentType.TEXT_ONLY).Subscribe(
                bookManifest => {
                    virtualFileLibrary.RetrieveBookMetaInfo(bookManifest.bookId).Subscribe(retrievedBookMetaInfo => {
                        Assert.Equals(bookMetaInfo.title, retrievedBookMetaInfo.title);
                    });
                },
                error => Debug.Log(error));
        }
    }
}