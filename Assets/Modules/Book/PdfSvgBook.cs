using System.Collections.Generic;

namespace Modules.Book {
    
    public class PdfSvgBook : Book<SvgPage> {

        public List<SvgPage> pages { get; }

        public PdfSvgBook(Binding binding, List<SvgPage> pages) 
            : base(binding, BookFormat.PDF) {
            this.pages = pages;
        }

        public override void appendPage(SvgPage page) {
            pages.Add(page);
        }

        public override void addPageAt(SvgPage page, int index) {
            pages.Insert(index, page);
        }

        public override bool removePage(SvgPage page) {
            return pages.Remove(page);
        }

        public override void removePageAt(int index) {
            pages.RemoveAt(index);
        }

        public override List<SvgPage> getPages() {
            return pages;
        }

        public override SvgPage getPage(int pageNum) {
            return pages[pageNum];
        }

        public override int getPageCount() {
            return pages.Count;
        }
    }
}