using System;
using System.Collections.Generic;
using Modules.EReader;
using NUnit.Framework;

namespace Modules.Bridge.Tests {
    public class PersistenceTests {
        
        [Test]
        public void serializingLibrary() {
            BookImporter<string[]> bookImporter = new BookImporter<string[]>();
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
            Book book = bookImporter.loadDotText("Assets/Modules/EReader/Tests/Resources/dracula.txt", bookMetaInfo);
            
            Shelf shelf = new Shelf("test shelf", new HashSet<string>{book.bookId});
            Library.Instance.addShelf(shelf);
            Library.Instance.serialize();
        }

        [Test]
        public void deserializeLibrary() {
            Library library = Library.Instance;
            Assert.NotNull(library);
        }
        
    }
}