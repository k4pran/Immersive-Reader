using System.Collections.Generic;

namespace EReader {
    
    public abstract class Book<T> {

        public abstract BookMetaInfo BookMetaInfo { get; }
        public abstract Binding binding { get; }

        public abstract void nextPage();
        public abstract void previousPage();
        public abstract void goTo(int index);
        public abstract List<Page<T>> getAllPages();
        public abstract List<Page<T>> getDisplayedPages();
        public abstract int getPageCount();
    }
}
