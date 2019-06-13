using System.Collections.Generic;
using EReader;
using UnityEngine;

namespace Bridge {
    
    public class Shelf {
        
        public string shelfName { get; set; }
        public HashSet<string> bookIds { get; set; }
        
        public Shelf() {}

        public Shelf(string shelfName, HashSet<string> bookIds) {
            this.shelfName = shelfName;
            this.bookIds = bookIds;
        }

        public void addToShelf(Book book) {
            bookIds.Add(book.bookId);
        }
        
        public void removeFromShelf(Book book) {
            bookIds.Remove(book.bookId);
        }
    }
}