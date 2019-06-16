using System;
using System.IO;
using NUnit.Framework;

namespace Modules.EReader.Tests {
    
    public class BookBuilderTests {
        
        [Test]
        public void buildingBasicBook() {
            string[] lines = File.ReadAllLines("Assets/Modules/EReader/Tests/Resources/dracula.txt");

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
            Book book = BookBuilder.buildBasicBook(lines, 27, bookMetaInfo);
            Assert.AreEqual(typeof(BasicBook), book.GetType());
        }
    }
}