using System.Collections.Generic;

namespace Modules.Book {

    public class SvgBook : Book<SvgPage> {

        public SvgBook(BookMetaInfo bookMetaInfo, Binding binding, List<SvgPage> pages)
            : base(bookMetaInfo, binding, Book.BookFormat.PDF) {
            this.pages = pages;
        }

        public List<SvgPage> pages { get; }

        public override IEnumerable<SvgPage> Pages() {
            return pages;
        }

        public override SvgPage Page(int pageNum) {
            return pages[pageNum];
        }

        public override int PageCount() {
            return pages.Count;
        }
    }
}