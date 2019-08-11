using System;
using System.Collections.Generic;
using Modules.Common;

namespace Modules.Book {
    
    public class ImagePage : Page<string> {
        
        public string imagePath { get; }

        
        public ImagePage(string pageName, int pageNb, string imagePath)
            : base (pageName, pageNb) {
            this.imagePath = imagePath;
        } 

        public override string getContent() {
            return imagePath;
        }

        public override ContentType getContentType() {
            return ContentType.IMAGE;
        }
    }
}