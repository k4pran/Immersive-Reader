using System;

namespace Modules.Library {

    public class InvalidBookIdException : Exception {

        public InvalidBookIdException() {
        }

        public InvalidBookIdException(string message)
            : base(message) {
        }
    }
}