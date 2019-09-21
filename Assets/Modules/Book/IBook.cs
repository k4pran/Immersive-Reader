using System.Collections.Generic;

namespace Modules.Book {

    public interface IBook<T> where T : Page {
        
        BookMetaInfo getBookMetaInfo();
        
        BookFormat getBookFormat();

        Binding getBinding();
        
        List<T> getPages();
        
        T getPage(int pageNum);
        
        int getPageCount();

        void appendPage(T page);

        void addPageAt(T page, int index);
        
        bool removePage(T page);

        void removePageAt(int index);
    }
}