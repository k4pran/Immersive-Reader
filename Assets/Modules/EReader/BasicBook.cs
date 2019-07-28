using System;
using System.Collections.Generic;

namespace Modules.EReader {
    
    public class BasicBook : Book<PageLines> {
        
        public List<Page<PageLines>> pages;
        public int linesPerPage;
        
        public BasicBook(string originUrl) 
                : base(originUrl, Binding.DOUBLE_PAGED, BookFormat.TEXT) {
        }

        public BasicBook(string originUrl, BookMetaInfo bookMetaInfo, int linesPerPage)
                : base(originUrl, Binding.DOUBLE_PAGED, BookFormat.TEXT) {
            this.linesPerPage = linesPerPage;
            setBookMetaInfo(bookMetaInfo);
            pages = new List<Page<PageLines>>();
        }

        public override void appendPage(Page<PageLines> page) {
            pages.Add(page);
        }

        public override void addPageAt(Page<PageLines> page, int index) {
            pages.Insert(index, page);
        }

        public override bool removePage(Page<PageLines> page) {
            return pages.Remove(page);
        }

        public override void removePageAt(int index) {
            pages.RemoveAt(index);
        }

        public override List<Page<PageLines>> getPages() {
            return pages;
        }

        public override Page<PageLines> getPage(int pageNum) {
            return pages[pageNum];
        }

        public override int getPageCount() {
            return pages.Count;
        }
    }
}