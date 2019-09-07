using Modules.Book.Tests.Common;

namespace Modules.Book {
    public interface IDynamicContent<out T> {
        
        T getContent();
        
        ContentType getContentType();
    }
}