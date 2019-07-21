using System;
using System.Collections.Generic;
using Modules.Common;

namespace Modules.EReader {
    
    public class BasicPage : Page {
        public override string pageName { get; set; }
        public override int pageNb { get; set; }

        public List<string> lines { get; set; }

        public BasicPage() {}

        public BasicPage(string[] lines, int pageNb) {
            this.lines = new List<string>(lines);
            this.pageNb = pageNb;
            pageName = pageNb.ToString();
        } 

        public override object getContent() {
            return lines;
        }

        public override ContentType getContentType() {
            return ContentType.TEXT_ONLY;
        }
    }
}