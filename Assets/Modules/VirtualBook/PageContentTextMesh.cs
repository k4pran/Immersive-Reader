using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace Modules.VirtualBook {
    public class PageContentTextMesh : PageContent {

        public TextMeshProUGUI tmpText;

        private void Awake() {
            tmpText.fontSizeMin = 8;
            tmpText.fontSizeMax = 200;
            tmpText.color = Color.black;
            tmpText.enableAutoSizing = true;
        }

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