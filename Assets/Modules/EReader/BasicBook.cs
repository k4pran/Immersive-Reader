using System.Collections.Generic;

namespace Modules.EReader {
    
    public class BasicBook : Book, IDynamicPages {
        public override string bookId { get; protected set; }
        public override BookMetaInfo bookMetaInfo { get; set; }
        public override Binding binding { get; protected set; }

        private string originUrl;
        public int linesPerPage;
        
        public List<Page> pages { get; set; }
        
        private BookFormat bookFormat;
        
        public BasicBook() {}

        public BasicBook(string originUrl) {
            this.originUrl = originUrl;
        }

        public BasicBook(string originUrl, BookMetaInfo bookMetaInfo, int linesPerPage, BookFormat bookFormat) {
            this.originUrl = originUrl;
            bookId = generateId();
            this.linesPerPage = linesPerPage;
            this.bookMetaInfo = bookMetaInfo;
            binding = Binding.DOUBLE_PAGED;
            pages = new List<Page>();
            this.bookFormat = bookFormat;
        }
        
        public BasicBook(string originUrl, BookMetaInfo bookMetaInfo, List<Page> pages, int linesPerPage, BookFormat bookFormat) {
            this.originUrl = originUrl;
            bookId = generateId();
            this.linesPerPage = linesPerPage;
            this.bookMetaInfo = bookMetaInfo;
            binding = Binding.DOUBLE_PAGED;
            this.pages = pages;
            this.bookFormat = bookFormat;
        }
        
        public void appendPage(Page page) {
            pages.Add(page);
        }

        public void addPageAt(Page page, int index) {
            pages.Insert(index, page);
        }

        public void removePage(int index) {
            pages.RemoveAt(index);
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

        public override BookFormat getBookFormat() {
            return bookFormat;
        }
    }
}