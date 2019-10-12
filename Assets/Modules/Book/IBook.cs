using System.Collections.Generic;

namespace Modules.Book {

    public interface IBook<out T> where T : Page {

        BookMetaInfo BookMetaInfo();

        BookFormat BookFormat();

        Binding Binding();

        IEnumerable<T> Pages();

        T Page(int pageNum);

        int PageCount();
    }
}