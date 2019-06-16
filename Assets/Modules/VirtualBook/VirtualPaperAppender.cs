using System.Text;
using TMPro;
using UnityEngine;

namespace Modules.VirtualBook {
    
    public class VirtualPaperAppender : MonoBehaviour {

        public static void asTextOnly(VirtualBookEntity bookEntity, string[] lines, string frontPageName, string backPageName="blank") {
            
            GameObject tmp = createTmpGui("content", lines);
            Paper paper = new Paper();
            VirtualPaper virtualPaper = new VirtualPaper(bookEntity, paper, frontPageName, backPageName, tmp);
        }
        
        public static void asTextOnly(VirtualBookEntity bookEntity, string[] frontLines, string[] backLines,  string frontPageName, string backPageName) {
            
            GameObject tmpFront = createTmpGui(frontPageName, frontLines);
            GameObject tmpBack = createTmpGui(backPageName, backLines);
            Paper paper = new Paper();
            VirtualPaper virtualPaper = new VirtualPaper(bookEntity, paper, frontPageName, backPageName, tmpFront, tmpBack);
        }

        public static GameObject createTmpGui(string name, string[] lines = null) {
            StringBuilder content = new StringBuilder();
            if (lines == null) lines = new string[0];
            foreach(string line in lines) {
                content.Append(line);
                content.Append("\n");
            }
            GameObject tmpWrapper = new GameObject(name);
            TextMeshProUGUI tmp = tmpWrapper.AddComponent<TextMeshProUGUI>();
            tmp.text = content.ToString();
            return tmpWrapper;
        }

        private void addPaper(VirtualBookEntity virtualBookEntity, Paper paper) {
            virtualBookEntity.appendPage(paper);
        }
    }
}