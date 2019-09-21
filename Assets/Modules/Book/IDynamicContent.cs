using System;
using Modules.Common;

namespace Modules.Book {
    
    public interface IDynamicContent {
        
        T getContent<T>();

        Type getContentClassType();
        
        ContentType getContentType();
    }
}