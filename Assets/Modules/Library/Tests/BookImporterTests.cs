using System;
using System.Collections.Generic;
using Modules.Book;
using NUnit.Framework;
using UnityEngine;

namespace Modules.Library.Tests {
    public class BookImporterTests {

        [Test]
        public void importBasicBook() {
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
            BasicBook book = new BasicBook("/Users/ryan/Documents/Unity/VReader_2/Assets/Modules/Book/Tests/Resources/dracula.txt", bookMetaInfo, 27);
            BookImporter<BasicBook> bookImporter = new BookImporter<BasicBook>();
            bookImporter.importFromLocal(book);
            VirtualFileLibrary vfl = new VirtualFileLibrary();
            vfl.setup();
            vfl.importBook(new Uri("file:///Users/ryan/Documents/Unity/VReader_2/Assets/Modules/Book/Tests/Resources/dracula.txt"), bookMetaInfo);
            Debug.Log("");
        }

        [Test]
        public void importPdf() {
            BookImporter<List<string>> bookImporter = new BookImporter<List<string>>();
            
            BookMetaInfo bookMetaInfo = new BookMetaInfo();
            bookMetaInfo.title = "Atari";
            bookMetaInfo.author = "Deepmind";
            bookMetaInfo.tags = new[] {"AI", "games"};
            bookMetaInfo.language = "English";
            bookMetaInfo.category = "Technical";
            bookMetaInfo.description = "Deepminds experiments with atari and RL";
            
            PdfSvgBook book = new PdfSvgBook("/Users/ryan/Documents/Books/atari.pdf", bookMetaInfo);
            bookImporter.importFromLocal(book);
        }
    }
}