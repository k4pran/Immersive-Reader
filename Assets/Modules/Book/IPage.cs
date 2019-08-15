using System;
using Modules.Book.Tests.Common;

namespace Modules.Book.Tests.Book {
    
    public interface IPage {

        string getPageName();
        
        int getPageNb();
    }
}