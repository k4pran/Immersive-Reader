using System.Collections.Generic;
using Modules.Book;
using Modules.Common;

namespace Modules.Library {

    public class BasicBookFactory {

        public class Builder : IBookFactory<BasicBook, TextPage> {

            private Binding binding = Binding.DOUBLE_PAGED;
            private readonly BookMetaInfo bookMetaInfo;

            private readonly string[] content;
            private int linesPerPage = BasicBook.LINES_PER_PAGE_DEFAULT;

            public Builder(string[] content, BookMetaInfo bookMetaInfo) {
                this.content = content;
                this.bookMetaInfo = bookMetaInfo;
            }

            public BasicBook Build() {
                var pages = GeneratePages();
                return new BasicBook(bookMetaInfo, binding, pages);
            }

            public Builder SetLinesPerPage(int linesPerPage) {
                this.linesPerPage = linesPerPage < BasicBook.LINES_PER_PAGE_MIN
                    ? BasicBook.LINES_PER_PAGE_MIN
                    : linesPerPage;
                return this;
            }

            public Builder SetBinding(Binding binding) {
                this.binding = binding;
                return this;
            }

            private List<TextPage> GeneratePages() {
                var pages = new List<TextPage>();
                var pageNb = 1;
                for (var i = 0; i < content.Length; i += linesPerPage) {
                    SubArray<string> pageContent;
                    if (i + linesPerPage >= content.Length)
                        pageContent = new SubArray<string>(content, i, content.Length - i);
                    else
                        pageContent = new SubArray<string>(content, i, linesPerPage);
                    pages.Add(GeneratePage(pageNb, pageContent));
                    pageNb++;
                }

                return pages;
            }

            private TextPage GeneratePage(int pageNb, SubArray<string> pageContent) {
                var pageName = string.Format("Page {0}", pageNb);
                var lines = pageContent.ToArray();
                return new TextPage(pageName, pageNb, lines);
            }

        }
    }
}