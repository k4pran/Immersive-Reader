using System;
using Modules.Book;
using Modules.Common;
using NUnit.Framework;
using UnityEngine;

namespace Modules.Library.Tests {

    public class BookBuilderTests {

        [Test]
        public void BuildBasicBook() {
            var bookMetaInfo = new BookMetaInfo();
            bookMetaInfo.title = "Dracula";
            bookMetaInfo.author = "Bram Stoker";
            bookMetaInfo.publisher = "Archibald Constable and Company (UK)";
            bookMetaInfo.pageCount = 368;
            bookMetaInfo.language = "English";
            bookMetaInfo.description = "Dracula is an 1897 Gothic horror novel by Irish author Bram Stoker";
            bookMetaInfo.publicationDate = new DateTime(1987, 5, 26);
            bookMetaInfo.category = "Gothic horror";
            bookMetaInfo.tags = new[] {"gothic", "horror", "vampires", "classic"};

            var virtualFileLibrary = new VirtualFileLibrary();
            var uri = new Uri(
                "file:///Users/ryan/Documents/Unity/VReader_2/Assets/Modules/Book/Tests/Resources/dracula.txt");
            virtualFileLibrary.ImportBook(uri, bookMetaInfo, ContentType.TEXT_ONLY).Subscribe(bookManifest => {
                    virtualFileLibrary.ReadBookAsLines(bookManifest.bookId).Subscribe(content => {
                            var basicBook = new BasicBookFactory.Builder(content, bookMetaInfo)
                                .SetLinesPerPage(27)
                                .Build();
                            Assert.AreEqual(27, basicBook.linesPerPage);
                            Assert.True(basicBook.pages.Count > 0);
                        },
                        error => Debug.Log(error));
                },
                error => Debug.Log(error));
        }


        [Test]
        public void TestPdfToSvg() {
            var uri = new Uri(
                "file:///Users/ryan/Documents/Unity/VReader_2/Assets/Modules/Book/Tests/Resources/atari.pdf");
            PdfConversion.ToSvgs(uri.AbsolutePath, "/Users/ryan/Documents/Unity/VReader_2/Assets/Modules/Library/Tests",
                "atari");
        }
    }
}