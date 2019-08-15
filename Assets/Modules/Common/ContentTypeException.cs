using System;

namespace Modules.Book.Tests.Common {
    public class ContentTypeException : Exception {
        public ContentTypeException() {
        }

        public ContentTypeException(string message) : base(message) {
        }
    }
}