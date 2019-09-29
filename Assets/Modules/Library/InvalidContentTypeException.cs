using System;

namespace Modules.Library {

    public class InvalidContentTypeException : Exception {

        public InvalidContentTypeException() {
        }

        public InvalidContentTypeException(string message)
            : base(message) {
        }

    }

}