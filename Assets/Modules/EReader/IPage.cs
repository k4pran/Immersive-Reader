using System;
using Modules.Common;

namespace Modules.EReader {
    
    public interface IPage {

        string getPageName();
        
        int getPageNb();
    }
}