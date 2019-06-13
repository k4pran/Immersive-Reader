using System;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;

namespace EReader {
    
    public class BasicBook : Book, IDynamicPages {
        public override string bookId { get; protected set; }
        public override BookMetaInfo bookMetaInfo { get; set; }
        public override Binding binding { get; protected set; }

        public int linesPerPage;
        
        public List<Page> pages { get; set; }
        
        private int pageIndex;

        private BookFormat bookFormat;
        
        public BasicBook() {}

        public BasicBook(BookMetaInfo bookMetaInfo, int linesPerPage, BookFormat bookFormat) {
            bookId = generateId();
            this.linesPerPage = linesPerPage;
            this.bookMetaInfo = bookMetaInfo;
            binding = Binding.DOUBLE_PAGED;
            pages = new List<Page>();
            pageIndex = 0;
            this.bookFormat = bookFormat;
        }
        
        public BasicBook(BookMetaInfo bookMetaInfo, List<Page> pages, int linesPerPage, BookFormat bookFormat) {
            bookId = generateId();
            this.linesPerPage = linesPerPage;
            this.bookMetaInfo = bookMetaInfo;
            binding = Binding.DOUBLE_PAGED;
            this.pages = pages;
            pageIndex = 0;
            this.bookFormat = bookFormat;
        }

        public void addPageAt(Page page, int index) {
            pages.Insert(index, page);
        }

        public void removePage(int index) {
            pages.RemoveAt(index);
        }

        public override void nextPage() {
            if (pageIndex % 2 == 0) goTo(pageIndex + 2);
            else goTo(pageIndex + 1);
        }

        public override void previousPage() {
            if (pageIndex % 2 == 0) goTo(pageIndex - 1);
            else goTo(pageIndex - 2);
        }

        public override void goTo(int index) {
            if (index < 0) pageIndex = 0;
            else if (index > pages.Count) pageIndex = pages.Count;
            else pageIndex = index;
        }

        public override List<Page> getAllPages() {
            return pages;
        }

        public override Page getPage(int pageNum) {
            return pages[pageNum];
        }

        public override List<Page> getDisplayedPages() {
            if (pageIndex % 2 == 0) return pages.GetRange(pageIndex, 2);
            else return pages.GetRange(pageIndex - 1, 2);
        }

        public override int getPageCount() {
            return pages.Count;
        }

        public override BookFormat getBookFormat() {
            return bookFormat;
        }

        public override int getPageNumber() {
            return pageIndex + 1;
        }
    }
}