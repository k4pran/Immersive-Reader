using System;
using Modules.Common;

namespace Modules.Book {

    public abstract class Page : IPage, IDynamicContent {

        private readonly string pageName;
        private readonly int pageNb;

        protected Page(string pageName, int pageNb) {
            this.pageName = pageName;
            this.pageNb = pageNb;
        }

        public abstract T Content<T>();

        public abstract Type ContentClassType();

        public abstract ContentType ContentType();

        public string PageName() {
            return pageName;
        }

        public int PageNb() {
            return pageNb;
        }
    }
}