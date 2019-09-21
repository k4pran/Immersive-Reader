using Modules.Book;

namespace Modules.Library {
    
    public interface IBookFactory<TBookType, TPageType> where TBookType : Book<TPageType> where TPageType : Page {

        TBookType build();

    }
}