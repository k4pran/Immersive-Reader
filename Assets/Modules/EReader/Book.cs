using System;
using System.Collections.Generic;

namespace Modules.EReader {
    
    public abstract class Book<T> : IBook<T> where T : Content {

        private string GUID_FORMAT = "D";
        private string bookId;

        private string originUrl;
        private BookMetaInfo bookMetaInfo;
        private Binding binding;
        private BookFormat bookFormat;


        protected Book(string originUrl, Binding binding, BookFormat bookFormat) {
            bookId = generateId();
            this.originUrl = originUrl;
            this.binding = binding;
            this.bookFormat = bookFormat;
        }

        protected string generateId() {
            return Guid.NewGuid().ToString(GUID_FORMAT);
        }

        public string getBookId() {
            if (bookId == null || bookId.Length == 0) {
                generateId();
            }
            return bookId;
        }

        public string getOriginUrl() {
            return originUrl;
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

        protected void setBookMetaInfo(BookMetaInfo bookMetaInfo) {
            this.bookMetaInfo = bookMetaInfo;
        }

        public abstract Page<T> getPage(int pageNum);
        
        public abstract List<Page<T>> getPages();

        public abstract int getPageCount();

        public abstract void appendPage(Page<T> page);

        public abstract void addPageAt(Page<T> page, int index);
        
        public abstract bool removePage(Page<T> page);

        public abstract void removePageAt(int index);
    }
}