using System.Collections.Generic;
using UnityEngine;

namespace EReader {
    
    public class Shelf {
        
        public string shelfName { get; set; }
        public List<Book> books { get; private set; }
        
        public Shelf() {}

        public Shelf(List<Book> books, string shelfName) {
            this.books = books;
            this.shelfName = shelfName;
        }
    }
}