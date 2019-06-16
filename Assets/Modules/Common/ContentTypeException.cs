using System;

namespace Modules.Common {
    public class ContentTypeException : Exception {
        public ContentTypeException() {
        }

        public ContentTypeException(string message) : base(message) {
        }
    }
}