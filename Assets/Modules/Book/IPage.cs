using System;
using Modules.Common;

namespace Modules.Book {
    
    public interface IPage {

        string getPageName();
        
        int getPageNb();
    }
}