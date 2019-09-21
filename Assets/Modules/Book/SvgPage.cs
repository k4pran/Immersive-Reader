
using System;
using Modules.Common;

namespace Modules.Book {
    
    public class SvgPage : Page {
        
        public string svg { get; }
        
        public SvgPage(string pageName, int pageNb, string svg)
            : base (pageName, pageNb) {
            this.svg = svg;
        }

        public override T getContent<T>() {
            return (T) Convert.ChangeType(svg, typeof(T));
        }

        public override Type getContentClassType() {
            return typeof(string);
        }

        public override ContentType getContentType() {
            return ContentType.SVG;
        }
    }
}