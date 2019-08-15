using System;
using System.Collections.Generic;
using Modules.Book.Tests.Common;

namespace Modules.Book.Tests.Book {
    
    public class TextPage : Page<List<string>> {

        public List<string> linesContent { get; }
        
        public TextPage(string pageName, int pageNb, string[] lines)
                : base (pageName, pageNb) {
            linesContent = new List<string>(new List<string>(lines));
        } 
        
        public override ContentType getContentType() {
            return ContentType.TEXT_ONLY;
        }
        
        public override List<string> getContent() {
            return linesContent;
        }
    }
}