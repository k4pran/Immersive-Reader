
using System;
using Modules.Common;

namespace Modules.Book {
    
    public abstract class Page : IPage, IDynamicContent {

        private string pageName;
        private int pageNb;

        protected Page(string pageName, int pageNb) {
            this.pageName = pageName;
            this.pageNb = pageNb;
        }

        public string getPageName() {
            return pageName;
        }

        public int getPageNb() {
            return pageNb;
        }

        public abstract T getContent<T>();
        
        public abstract Type getContentClassType();

        public abstract ContentType getContentType();
    }
}