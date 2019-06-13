using System;
using System.Collections.Generic;

namespace EReader {
    
    public abstract class Book {
        protected Book() {}

        protected string generateId() {
            return Guid.NewGuid().ToString("D");
        }

        public abstract string bookId { get; protected set; }
        public abstract BookMetaInfo bookMetaInfo { get; set; }
        public abstract Binding binding { get; protected set; }
        
        public abstract void nextPage();
        public abstract void previousPage();
        public abstract void goTo(int index);
        public abstract List<Page> getAllPages();
        public abstract List<Page> getDisplayedPages();
        public abstract Page getPage(int pageNum);
        public abstract int getPageCount();
        
        public abstract BookFormat getBookFormat();

        public abstract int getPageNumber();
    }
}