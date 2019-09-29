using System;

namespace Modules.Common {

    public class UnsupportFileFormatException : Exception {

        public UnsupportFileFormatException() {
        }

        public UnsupportFileFormatException(string message)
            : base(message) {
        }
    }
}