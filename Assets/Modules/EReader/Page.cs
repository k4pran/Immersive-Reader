using System;
using Modules.Common;

namespace Modules.EReader {
    
    public abstract class Page : IDynamicContent {
        protected Page() {}

        public abstract string pageName { get; set; }
        public abstract int pageNb { get; set; }
        
        public abstract Object getContent();
        public abstract ContentType getContentType();
    }
}