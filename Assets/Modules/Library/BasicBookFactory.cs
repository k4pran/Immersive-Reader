using System;
using System.Collections.Generic;
using Modules.Book;
using Modules.Common;

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
                this.linesPerPage = linesPerPage < BasicBook.LINES_PER_PAGE_MIN ? BasicBook.LINES_PER_PAGE_MIN : 
                                                                                  linesPerPage;
                return this;
            }

            public Builder setBinding(Binding binding) {
                this.binding = binding;
                return this;
            }

            public BasicBook build() {
                List<TextPage> pages = generatePages();
                return new BasicBook(bookMetaInfo, binding, pages);
            }

            private List<TextPage> generatePages() {
                List<TextPage> pages = new List<TextPage>();
                int pageNb = 1;
                for (int i = 0; i < content.Length; i += linesPerPage) {
                    SubArray<string> pageContent;
                    if (i + linesPerPage >= content.Length) {
                        pageContent = new SubArray<string>(content, i, content.Length - i);
                    }
                    else {
                        pageContent = new SubArray<string>(content, i, linesPerPage);
                    }
                    pages.Add(generatePage(pageNb, pageContent));
                    pageNb++;
                }

                return pages;
            }

            private TextPage generatePage(int pageNb, SubArray<string> pageContent) {
                string pageName = String.Format("Page {0}", pageNb);
                string[] lines = pageContent.ToArray();
                return new TextPage(pageName, pageNb, lines);
            }
        }
    }
}