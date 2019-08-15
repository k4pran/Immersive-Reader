using Modules.Book.Tests.Common;

namespace Modules.Book.Tests.Book {
    
    public abstract class Page<T> : IPage, IDynamicContent<T> {

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

        public abstract T getContent();

        public abstract ContentType getContentType();
    }
}