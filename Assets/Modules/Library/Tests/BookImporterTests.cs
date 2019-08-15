using System.Collections.Generic;
using Modules.Book.Tests.Book;
using NUnit.Framework;

namespace Modules.Library.Tests {
    public class BookImporterTests {

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
            
            PdfBasicBook book = new PdfBasicBook("/Users/ryan/Documents/Books/atari.pdf", bookMetaInfo);
            bookImporter.importFromLocal(book);
        }
    }
}