namespace Modules.Library {

    public class BookToken {

        public BookToken(string bookId, string bookTitle) {
            this.bookId = bookId;
            this.bookTitle = bookTitle;
        }

        public string bookId { get; }
        public string bookTitle { get; }
    }
}