namespace Modules.VirtualBook {

    public class DoublePageController : IBookController {

        public int pageCount { get; set; }
        
        public int currentPage { get; private set; }

        public DoublePageController(int pageCount, int currentPage = 0) {
            this.pageCount = pageCount;
            this.currentPage = currentPage;
        }

        public int CurrentPageNb() {
            return currentPage;
        }

        public int Next() {
            if (currentPage >= End() - 2) {
                currentPage = End();
                return CurrentPageNb();
            }

            currentPage += 2;
            return currentPage;
        }

        public int Previous() {
            if (currentPage <= 1) {
                currentPage = Start();
                return currentPage;
            }

            currentPage -= 2;
            return currentPage;
        }

        public int GoTo(int pageNb) {
            if (pageNb < Start()) {
                currentPage = Start();
                return currentPage;
            }

            if (pageNb > End()) {
                currentPage = End();
                return currentPage;
            }

            currentPage = pageNb;
            return currentPage;
        }

        public int Start() {
            currentPage = 0;
            return currentPage;
        }

        public int End() {
            currentPage = pageCount - 1;
            return currentPage;
        }
    }
}