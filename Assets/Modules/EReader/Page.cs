using System;

namespace EReader {
    
    public abstract class Page : IDynamicContent {
        protected Page() {}

        public abstract string pageName { get; set; }
        public abstract int pageNb { get; set; }
        
        public abstract double topMargin { get; set; }
        public abstract double bottomMargin { get; set; }
        public abstract double rightMargin { get; set; }
        public abstract double leftMargin { get; set; }
        
        public abstract Object getContent();
        public abstract Type getContentType();
    }
}