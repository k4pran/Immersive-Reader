using System;
using Modules.Common;

namespace Modules.Book {

    public class SvgPage : Page {

        public SvgPage(string pageName, int pageNb, string svg)
            : base(pageName, pageNb) {
            this.svg = svg;
        }

        public string svg { get; }

        public override T Content<T>() {
            return (T) Convert.ChangeType(svg, typeof(T));
        }

        public override Type ContentClassType() {
            return typeof(string);
        }

        public override ContentType ContentType() {
            return Common.ContentType.SVG;
        }
    }
}