using Modules.Common;

namespace Modules.EReader {
    public interface IDynamicContent<T> {
        
        T getContent();
        
        ContentType getContentType();
    }
}