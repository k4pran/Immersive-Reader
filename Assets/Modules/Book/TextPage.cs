using System;
using System.Collections.Generic;
using System.Linq;
using Modules.Common;

namespace Modules.Book {
    
    public class TextPage : Page<List<string>> {

        private static readonly char COLLAPSE_DELIMITER = '|';
        public List<string> linesContent { get; }

        public TextPage(string pageName, int pageNb, string[] lines)
                : base (pageName, pageNb) {
            linesContent = new List<string>(new List<string>(lines));
        } 
        
        public string collapseLines() {
            return String.Join(COLLAPSE_DELIMITER.ToString(), linesContent);
        }

        private List<string> splitLines(string lines) {
            return lines.Split(COLLAPSE_DELIMITER).ToList();
        }
        
        public override ContentType getContentType() {
            return ContentType.TEXT_ONLY;
        }
        
        public override List<string> getContent() {
            return linesContent;
        }
    }
}