using System;
using System.Collections.Generic;
using Modules.Common;

namespace Modules.EReader {
    
    public class ImagePage : Page {
        public override string pageName { get; set; }
        public override int pageNb { get; set; }
        public string imagePath { get; set; }

        
        public ImagePage() {}
        
        public ImagePage(string imagePath, int pageNb) {
            this.imagePath = imagePath;
            this.pageNb = pageNb;
        }

        public override object getContent() {
            return imagePath;
        }

        public override ContentType getContentType() {
            return ContentType.IMAGE;
        }
    }
}