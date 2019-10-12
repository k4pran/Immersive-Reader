using System.Collections.Generic;

namespace Modules.Book {

    public abstract class Book<T> : IBook<T> where T : Page {

        private readonly Binding binding;
        private readonly BookFormat bookFormat;

        private readonly BookMetaInfo bookMetaInfo;

        protected Book(BookMetaInfo bookMetaInfo, Binding binding, BookFormat bookFormat) {
            this.bookMetaInfo = bookMetaInfo;
            this.binding = binding;
            this.bookFormat = bookFormat;
        }

        public BookMetaInfo BookMetaInfo() {
            return bookMetaInfo;
        }

        public Binding Binding() {
            return binding;
        }

        public BookFormat BookFormat() {
            return bookFormat;
        }

        public abstract T Page(int pageNum);

        public abstract IEnumerable<T> Pages();

        public abstract int PageCount();
    }
}