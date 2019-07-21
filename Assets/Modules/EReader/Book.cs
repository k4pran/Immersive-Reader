using System;
using System.Collections.Generic;

namespace Modules.EReader {
    
    public abstract class Book {

        private string GUID_FORMAT = "D";
        protected Book() {}

        protected string generateId() {
            return Guid.NewGuid().ToString(GUID_FORMAT);
        }

        public abstract string bookId { get; protected set; }
        public abstract BookMetaInfo bookMetaInfo { get; set; }
        public abstract Binding binding { get; protected set; }
        public abstract List<Page> getAllPages();
        public abstract Page getPage(int pageNum);
        public abstract int getPageCount();

        public abstract string getOriginUrl();
        
        public abstract BookFormat getBookFormat();
    }
}