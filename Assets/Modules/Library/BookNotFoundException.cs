using System;

namespace Modules.Library {
    
    public class BookNotFoundException : Exception {

        public BookNotFoundException() {}

        public BookNotFoundException(string message)
            : base(message) {}
    }
}