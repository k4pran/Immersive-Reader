using System;
using Modules.Common;

namespace Modules.EReader {
    
    public interface IDynamicContent {
        
        Object getContent();
        ContentType getContentType();
    }
}