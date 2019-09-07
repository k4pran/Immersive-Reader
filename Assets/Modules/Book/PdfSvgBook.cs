using System.Collections.Generic;

namespace Modules.Book {
    
    public class PdfSvgBook : Book<string> {

        public List<Page<string>> pages { get; }
        private BookFormat bookFormat;
        
        public PdfSvgBook(string originUrl, BookMetaInfo bookMetaInfo)
            : base(originUrl, Binding.DOUBLE_PAGED, BookFormat.PDF) {
            setBookMetaInfo(bookMetaInfo);
            pages = new List<Page<string>>();
            setBookMetaInfo(new BookMetaInfo());
        }
        
        public PdfSvgBook(Book<string> book)
            : base(book.getOriginUrl(), Binding.DOUBLE_PAGED, BookFormat.PDF) {
            setBookMetaInfo(book.getBookMetaInfo());
        }

        public override void appendPage(Page<string> page) {
            pages.Add(page);
        }

        public override void addPageAt(Page<string> page, int index) {
            pages.Insert(index, page);
        }

        public override bool removePage(Page<string> page) {
            return pages.Remove(page);
        }

        public override void removePageAt(int index) {
            pages.RemoveAt(index);
        }

        public override List<Page<string>> getPages() {
            return pages;
        }

        public override Page<string> getPage(int pageNum) {
            return pages[pageNum];
        }

        public override int getPageCount() {
            return pages.Count;
        }
    }
}