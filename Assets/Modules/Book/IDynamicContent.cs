using Modules.Book.Tests.Common;

namespace Modules.Book.Tests.Book {
    public interface IDynamicContent<out T> {
        
        T getContent();
        
        ContentType getContentType();
    }
}