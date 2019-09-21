using System.Collections.Generic;

namespace Modules.Book {
    
    public class BasicBook : Book<TextPage> {

        public static readonly int LINES_PER_PAGE_DEFAULT = 27;
        public static readonly int LINES_PER_PAGE_MIN = 12;

        public List<TextPage> pages;
        public int linesPerPage;
        
        public BasicBook(Binding binding, List<TextPage> pages) 
            : base(binding, BookFormat.TEXT) {
            this.pages = pages;
            linesPerPage = LINES_PER_PAGE_DEFAULT;
        }

        public BasicBook(Binding binding, BookFormat bookFormat, List<TextPage> pages, int linesPerPage)
            : base(binding, bookFormat) {
            this.pages = pages;
            this.linesPerPage = linesPerPage < LINES_PER_PAGE_MIN ? LINES_PER_PAGE_MIN : linesPerPage;
        }

        public override void appendPage(TextPage page) {
            pages.Add(page);
        }

        public override void addPageAt(TextPage page, int index) {
            pages.Insert(index, page);
        }

        public override bool removePage(TextPage page) {
            return pages.Remove(page);
        }

        public override void removePageAt(int index) {
            pages.RemoveAt(index);
        }

        public override List<TextPage> getPages() {
            return pages;
        }

        public override TextPage getPage(int pageNum) {
            return pages[pageNum];
        }

        public override int getPageCount() {
            return pages.Count;
        }
    }
}