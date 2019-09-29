using System.Collections.Generic;

namespace Modules.Book {

    public class BasicBook : Book<TextPage> {

        public static readonly int LINES_PER_PAGE_DEFAULT = 27;
        public static readonly int LINES_PER_PAGE_MIN = 12;
        public int linesPerPage;

        public List<TextPage> pages;

        public BasicBook(BookMetaInfo bookMetaInfo, Binding binding, List<TextPage> pages)
            : base(bookMetaInfo, binding, Book.BookFormat.TEXT) {
            this.pages = pages;
            linesPerPage = LINES_PER_PAGE_DEFAULT;
        }

        public BasicBook(BookMetaInfo bookMetaInfo, Binding binding, BookFormat bookFormat, List<TextPage> pages,
            int linesPerPage)
            : base(bookMetaInfo, binding, bookFormat) {
            this.pages = pages;
            this.linesPerPage = linesPerPage < LINES_PER_PAGE_MIN ? LINES_PER_PAGE_MIN : linesPerPage;
        }

        public override IEnumerable<TextPage> Pages() {
            return pages;
        }

        public override TextPage Page(int pageNum) {
            return pages[pageNum];
        }

        public override int PageCount() {
            return pages.Count;
        }
    }
}