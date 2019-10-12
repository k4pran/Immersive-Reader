using System;

namespace Modules.Library {

    public class InvalidOptionException : Exception {

        public InvalidOptionException() {
        }

        public InvalidOptionException(string message)
            : base(message) {
        }
    }
}