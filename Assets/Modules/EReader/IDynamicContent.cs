using System;

namespace Modules.EReader {
    
    public interface IDynamicContent {
        
        Object getContent();
        Type getContentType();
    }
}