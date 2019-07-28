using System.Collections.Generic;

namespace Modules.EReader {
    
    public class PageLines : Content {

        private List<string> lines;

        public PageLines(List<string> lines) {
            this.lines = lines;
        }

        public int getLineCount() {
            return lines.Count;
        }
    }
}