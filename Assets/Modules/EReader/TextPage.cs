using System;
using System.Collections.Generic;
using Modules.Common;

namespace Modules.EReader {
    
    public class TextPage : Page<Content> {

        public PageLines lines { get; }
        
        public TextPage(string pageName, int pageNb, string[] lines)
                : base (pageName, pageNb) {
            this.lines = new PageLines(new List<string>(lines));
        } 
        
        public override ContentType getContentType() {
            return ContentType.TEXT_ONLY;
        }
        
        public override Content getContent() {
            return lines;
        }
    }
}