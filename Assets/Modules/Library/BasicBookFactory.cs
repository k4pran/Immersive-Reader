using System;
using System.Collections.Generic;
using Modules.Book;

namespace Modules.Library {
    
    public class BasicBookFactory {

        public class Builder : IBookFactory<BasicBook, TextPage> {

            private string[] content;
            private BookMetaInfo bookMetaInfo;
            private int linesPerPage = BasicBook.LINES_PER_PAGE_DEFAULT;
            private Binding binding = Binding.DOUBLE_PAGED;

            public Builder(string[] content, BookMetaInfo bookMetaInfo) {
                this.content = content;
                this.bookMetaInfo = bookMetaInfo;
            }

            public Builder setLinesPerPage(int linesPerPage) {
                this.linesPerPage = linesPerPage;
                return this;
            }

            public Builder setBinding(Binding binding) {
                this.binding = binding;
                return this;
            }

            public BasicBook build() {
                List<TextPage> pages = generatePages();
                return new BasicBook(binding, pages);
            }

            private List<TextPage> generatePages() {
                List<TextPage> pages = new List<TextPage>();
                int pageNb = 1;
                for (int i = 0; i < content.Length; i += linesPerPage) {
                    ArraySegment<string> pageContent = new ArraySegment<string>(content, i, linesPerPage);
                    pages.Add(generatePage(pageNb, pageContent));
                    pageNb++;
                }

                return pages;
            }

            private TextPage generatePage(int pageNb, ArraySegment<string> pageContent) {
                return null;
            }
        }
    }
}