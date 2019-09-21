using System;
using Modules.Book;
using NUnit.Framework;
using UnityEngine;

namespace Modules.Library.Tests {
    
    public class BookBuilderTests {

        [Test]
        public void buildBasicBook() {
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
                    virtualFileLibrary.readBookAsLines(bookManifest.bookId).Subscribe(content => {
                        BasicBook basicBook = new BasicBookFactory.Builder(content, bookMetaInfo)
                            .setLinesPerPage(1)
                            .build();
                    });
                },
                error => Debug.Log(error));
            
        }
        
    }
}