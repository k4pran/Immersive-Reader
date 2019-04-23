using System;
using System.Collections.Generic;

namespace EReader {
    
    public class TxtPage : Page {
        public override string pageName { get; set; }
        public override int pageNb { get; set; }
        public override double topMargin { get; set; }
        public override double bottomMargin { get; set; }
        public override double rightMargin { get; set; }
        public override double leftMargin { get; set; }

        public List<string> lines { get; set; }

        public TxtPage() {}

        public TxtPage(string[] lines, int pageNb) {
            this.lines = new List<string>(lines);
            this.pageNb = pageNb;
            pageName = pageNb.ToString();
        } 

        public TxtPage(List<string> lines, int pageNb) {
            this.lines = lines;
            this.pageNb = pageNb;
            pageName = pageNb.ToString();
        }

        public override object getContent() {
            return lines;
        }

        public override Type getContentType() {
            return typeof(List<string>);
        }
    }
}