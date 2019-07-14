using System.Collections.Generic;
using System.Text;
using TMPro;

namespace Modules.VirtualBook {
    public class PageContentTextMesh : PageContent {

        public TMP_Text tmpText;

        public void setText(List<string> lines) {
            
            StringBuilder content = new StringBuilder();
            foreach(string line in lines) {
                content.Append(line);
                content.Append("\n");
            }
            tmpText.text = content.ToString();
        }
    }
}