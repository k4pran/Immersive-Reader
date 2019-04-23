using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace EReader.Tests {
    public class PersistenceTests {
        
        [Test]
        public void serializingLibrary() {
            BookImporter<string[]> bookImporter = new BookImporter<string[]>();
            string[] contents = bookImporter.loadText("Assets/Modules/EReader/Tests/Resources/dracula.txt");
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
            Book book = BookBuilder.buildBasicBook(contents, 27, bookMetaInfo);
            
            Shelf shelf = new Shelf(new List<Book>(){book}, "test shelf");
            Library library = new Library(new List<Shelf>(){shelf});
            library.serialize();
        }

        [Test]
        public void deserializeLibrary() {
            Library library = Library.Deserialize();
            Debug.Log(library);
        }
        
    }
}