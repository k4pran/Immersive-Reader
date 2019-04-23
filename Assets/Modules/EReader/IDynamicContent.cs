using System;

namespace EReader {
    
    public interface IDynamicContent {
        
        Object getContent();
        Type getContentType();
    }
}