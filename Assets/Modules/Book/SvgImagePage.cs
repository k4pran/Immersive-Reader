using Modules.Book.Tests.Common;
using UnityEngine.UI;

namespace Modules.Book {
    
    public class SvgImagePage : Page<string> {
        
        public string image { get; }

        
        public SvgImagePage(string pageName, int pageNb, string image)
            : base (pageName, pageNb) {
            this.image = image;
        } 

        public override string getContent() {
            return image;
        }

        public override ContentType getContentType() {
            return ContentType.SVG;
        }
    }
}