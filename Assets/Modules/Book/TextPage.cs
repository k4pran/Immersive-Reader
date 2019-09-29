using System;
using System.Collections.Generic;
using System.Linq;
using Modules.Common;

namespace Modules.Book {

    public class TextPage : Page {

        private static readonly char COLLAPSE_DELIMITER = '|';

        public TextPage(string pageName, int pageNb, string[] lines)
            : base(pageName, pageNb) {
            linesContent = new List<string>(new List<string>(lines));
        }

        public List<string> linesContent { get; }

        public string CollapseLines() {
            return string.Join(COLLAPSE_DELIMITER.ToString(), linesContent);
        }

        private List<string> SplitLines(string lines) {
            return lines.Split(COLLAPSE_DELIMITER).ToList();
        }

        public override T Content<T>() {
            return (T) Convert.ChangeType(linesContent, typeof(T));
        }

        public override Type ContentClassType() {
            return typeof(List<string>);
        }

        public override ContentType ContentType() {
            return Common.ContentType.TEXT_ONLY;
        }
    }
}