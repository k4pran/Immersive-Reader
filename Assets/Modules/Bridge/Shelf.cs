using System.Collections.Generic;
using Modules.EReader;
using UnityEngine;

namespace Modules.Bridge {
    
    public class Shelf {

        public static Shelf allBooks;
                
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