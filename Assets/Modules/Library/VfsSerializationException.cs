using System;

namespace Modules.Library  {
    
    public class VfsSerializationException : Exception {
        
        public VfsSerializationException() {}

        public VfsSerializationException(string message)
            : base(message) {}
    }
}