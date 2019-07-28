using System;
using System.Collections.Generic;
using Modules.Common;

namespace Modules.EReader {
    
    public class ImagePage : Page<Content> {
        
        public ImageLocation imagePath { get; }

        
        public ImagePage(string pageName, int pageNb, string imagePath)
            : base (pageName, pageNb) {
            this.imagePath = new ImageLocation(imagePath);
        } 

        public override Content getContent() {
            return imagePath;
        }

        public override ContentType getContentType() {
            return ContentType.IMAGE;
        }
    }
}