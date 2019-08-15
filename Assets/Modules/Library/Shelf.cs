using System.Collections.Generic;
using Modules.Book.Tests.Book;
using UnityEngine;

namespace Modules.Library {
    
    public class Shelf {

        public static Shelf allBooks;
                
        public string shelfName { get; set; }
        public HashSet<string> bookIds { get; set; }
        
        public Shelf() {}

        public Shelf(string shelfName, HashSet<string> bookIds) {
            this.shelfName = shelfName;
            this.bookIds = bookIds;
        }

        public void addToShelf<T>(Book<T> book) {
            bookIds.Add(book.getBookId());
        }
        
        public void removeFromShelf<T>(Book<T> book) {
            bookIds.Remove(book.getBookId());
        }
    }
}