using System;

namespace DefaultNamespace {
    
    public class InvalidBookIDException : Exception {
        
        public InvalidBookIDException() {}

        public InvalidBookIDException(string message)
            : base(message) {}

    }
}