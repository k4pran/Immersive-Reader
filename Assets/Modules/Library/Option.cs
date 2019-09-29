using Modules.Book;

namespace Modules.Library {

    public enum Option {

        LINES_PER_PAGE,
        LINES_PER_PAGE_MINIMUM
    }

    internal static class OptionExtensions {

        public static object getOptionDefault(this Option option) {
            switch (option) {
                case Option.LINES_PER_PAGE:
                    return BasicBook.LINES_PER_PAGE_DEFAULT;

                case Option.LINES_PER_PAGE_MINIMUM:
                    return BasicBook.LINES_PER_PAGE_MIN;

                default:
                    throw new InvalidOptionException();
            }
        }
    }
}