using System.Collections.Generic;

namespace Modules.Book.Tests.Book {
    
    public class BasicBook : Book<List<string>> {
        
        public List<Page<List<string>>> pages;
        public int linesPerPage;
        
        public BasicBook(string originUrl) 
                : base(originUrl, Binding.DOUBLE_PAGED, BookFormat.TEXT) {
        }

        public BasicBook(string originUrl, BookMetaInfo bookMetaInfo, int linesPerPage)
                : base(originUrl, Binding.DOUBLE_PAGED, BookFormat.TEXT) {
            this.linesPerPage = linesPerPage;
            setBookMetaInfo(bookMetaInfo);
            pages = new List<Page<List<string>>>();
        }

        public BasicBook(Book<object> book, int linesPerPage)
                : base(book.getOriginUrl(), Binding.DOUBLE_PAGED, BookFormat.TEXT) {
            this.linesPerPage = linesPerPage;
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