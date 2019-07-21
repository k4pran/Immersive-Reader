using System.Collections.Generic;

namespace Modules.EReader {
    public class PdfBasicBook : Book {
        
        public override string bookId { get; protected set; }
        public override BookMetaInfo bookMetaInfo { get; set; }
        public override Binding binding { get; protected set; }

        private string originUrl;
        public List<Page> pages { get; set; }
        private int pageIndex;
        private BookFormat bookFormat;

        public PdfBasicBook() {
        }

        public PdfBasicBook(string originUrl) {
            this.originUrl = originUrl;
        }
        
        public PdfBasicBook(string originUrl, BookMetaInfo bookMetaInfo) {
            this.originUrl = originUrl;
            bookId = generateId();
            this.bookMetaInfo = bookMetaInfo;
            binding = Binding.DOUBLE_PAGED;
            pages = new List<Page>();
            bookFormat = BookFormat.PDF;
        }

        public override string getOriginUrl() {
            return originUrl;
        }

        public override List<Page> getAllPages() {
            return pages;
        }

        public override Page getPage(int pageNum) {
            return pages[pageNum];
        }

        public override int getPageCount() {
            return pages.Count;
        }

        public void appendPage(Page page) {
            pages.Add(page);
        }

        public override BookFormat getBookFormat() {
            return bookFormat;
        }
    }
}