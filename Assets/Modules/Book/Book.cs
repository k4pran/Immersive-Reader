using System.Collections.Generic;

namespace Modules.Book {
    
    public abstract class Book<T> : IBook<T> where T : Page {
        
        private BookMetaInfo bookMetaInfo;
        private Binding binding;
        private BookFormat bookFormat;
        
        protected Book(BookMetaInfo bookMetaInfo, Binding binding, BookFormat bookFormat) {
            this.bookMetaInfo = bookMetaInfo;
            this.binding = binding;
            this.bookFormat = bookFormat;
        }

        public BookMetaInfo getBookMetaInfo() {
            return bookMetaInfo;
        }

        public Binding getBinding() {
            return binding;
        }

        public BookFormat getBookFormat() {
            return bookFormat;
        }

        public abstract T getPage(int pageNum);

        public abstract List<T> getPages();

        public abstract int getPageCount();

        public abstract void appendPage(T page);

        public abstract void addPageAt(T page, int index);

        public abstract bool removePage(T page);

        public abstract void removePageAt(int index);
    }
}