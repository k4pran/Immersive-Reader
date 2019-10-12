using Modules.Book;

namespace Modules.Library {

    public interface IBookFactory<TBookType, TPageType> where TBookType : IBook<TPageType> where TPageType : Page {
        
        TBookType Build();
    }
}