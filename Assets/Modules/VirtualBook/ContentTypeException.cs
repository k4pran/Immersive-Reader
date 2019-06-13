using System;

namespace VirtualBook {
    public class ContentTypeException : Exception {
        public ContentTypeException() {
        }

        public ContentTypeException(string message) : base(message) {
        }
    }
}