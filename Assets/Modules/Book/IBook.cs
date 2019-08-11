using System.Collections.Generic;

namespace Modules.Book {

    public interface IBook<T> {

        string getBookId();

        BookMetaInfo getBookMetaInfo();
        
        BookFormat getBookFormat();

        Binding getBinding();
        
        List<Page<T>> getPages();
        
        Page<T> getPage(int pageNum);
        
        int getPageCount();

        void appendPage(Page<T> page);

        void addPageAt(Page<T> page, int index);
        
        bool removePage(Page<T> page);

        void removePageAt(int index);
    }
}