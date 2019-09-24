using System;
using System.Collections.Generic;
using Modules.Book;
using Modules.Common;

namespace Modules.Library {
    
    public class SvgBookFactory {
        
        public class Builder : IBookFactory<BasicBook, TextPage> {

            private string[] content;
            private BookMetaInfo bookMetaInfo;
            private Binding binding = Binding.DOUBLE_PAGED;

            public Builder(string[] content, BookMetaInfo bookMetaInfo) {
                this.content = content;
                this.bookMetaInfo = bookMetaInfo;
            }

            public Builder setBinding(Binding binding) {
                this.binding = binding;
                return this;
            }

            public BasicBook build() {
                throw new NotImplementedException();
            }

            private List<TextPage> generatePages() {
                throw new NotImplementedException();
            }

            private TextPage generatePage(int pageNb, SubArray<string> pageContent) {
                throw new NotImplementedException();
            }
        }
        
    }
}