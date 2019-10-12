using System;

namespace Modules.Book {

    public enum BookFormat {

        TEXT,
        PDF,
        UNKNOWN
    }

    public static class BookFormatUtils {

        public static string AsString(this BookFormat bookFormat) {
            switch (bookFormat) {
                case BookFormat.TEXT:
                    return ".txt";

                case BookFormat.PDF:
                    return ".pdf";

                default:
                    throw new BookFormatException("Unknown book format: " + bookFormat);
            }
        }

        public static BookFormat FromString(this string fileExt) {
            switch (fileExt) {
                case ".txt":
                    return BookFormat.TEXT;

                case ".pdf":
                    return BookFormat.PDF;

                default:
                    throw new BookFormatException("Unknown file extension: " + fileExt);
            }
        }

    }

    public class BookFormatException : Exception {

        public BookFormatException(string message) : base(message) {}
    }
}