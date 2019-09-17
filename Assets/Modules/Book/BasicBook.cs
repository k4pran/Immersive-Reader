using System.Collections.Generic;

namespace Modules.Book {
    
    public class BasicBook : Book<List<string>> {

        public readonly int LINES_PER_PAGE_DEFAULT = 27;
        public readonly int LINES_PER_PAGE_MIN = 21;

        public List<Page<List<string>>> pages;
        public int linesPerPage;

        public BasicBook(string originUrl, 
                         Binding binding, 
                         BookFormat bookFormat, 
                         List<Page<List<string>>> pages, 
                         int linesPerPage) 
            : base(originUrl, binding, bookFormat) {
            this.pages = pages;
            this.linesPerPage = linesPerPage;
        }

        public BasicBook(string originUrl) 
                : base(originUrl, Binding.DOUBLE_PAGED, BookFormat.TEXT) {
            pages = new List<Page<List<string>>>();
            linesPerPage = LINES_PER_PAGE_DEFAULT;
            setBookMetaInfo(new BookMetaInfo());
        }

        public BasicBook(string originUrl, BookMetaInfo bookMetaInfo, int linesPerPage)
                : base(originUrl, Binding.DOUBLE_PAGED, BookFormat.TEXT) {
            this.linesPerPage = linesPerPage < LINES_PER_PAGE_MIN ? LINES_PER_PAGE_MIN : linesPerPage;
            setBookMetaInfo(bookMetaInfo);
            pages = new List<Page<List<string>>>();
        }

        public BasicBook(Book<object> book, int linesPerPage)
                : base(book.getOriginUrl(), Binding.DOUBLE_PAGED, BookFormat.TEXT) {
            this.linesPerPage = linesPerPage < LINES_PER_PAGE_MIN ? LINES_PER_PAGE_MIN : linesPerPage;
            setBookMetaInfo(book.getBookMetaInfo());
            pages = new List<Page<List<string>>>();
        }

        public override void appendPage(Page<List<string>> page) {
            pages.Add(page);
        }

        public override void addPageAt(Page<List<string>> page, int index) {
            pages.Insert(index, page);
        }

        public override bool removePage(Page<List<string>> page) {
            return pages.Remove(page);
        }

        public override void removePageAt(int index) {
            pages.RemoveAt(index);
        }

        public override List<Page<List<string>>> getPages() {
            return pages;
        }

        public override Page<List<string>> getPage(int pageNum) {
            return pages[pageNum];
        }

        public override int getPageCount() {
            return pages.Count;
        }
    }
}