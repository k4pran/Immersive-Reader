using System.Collections.Generic;

namespace Modules.EReader {
    
    public class PdfBasicBook : Book<ImageLocation> {

        public List<Page<ImageLocation>> pages { get; }
        private BookFormat bookFormat;
        
        public PdfBasicBook(string originUrl, BookMetaInfo bookMetaInfo)
            : base(originUrl, Binding.DOUBLE_PAGED, BookFormat.PDF) {
            setBookMetaInfo(bookMetaInfo);
        }

        public override void appendPage(Page<ImageLocation> page) {
            pages.Add(page);
        }

        public override void addPageAt(Page<ImageLocation> page, int index) {
            pages.Insert(index, page);
        }

        public override bool removePage(Page<ImageLocation> page) {
            return pages.Remove(page);
        }

        public override void removePageAt(int index) {
            pages.RemoveAt(index);
        }

        public override List<Page<ImageLocation>> getPages() {
            return pages;
        }

        public override Page<ImageLocation> getPage(int pageNum) {
            return pages[pageNum];
        }

        public override int getPageCount() {
            return pages.Count;
        }
    }
}