using System;
using System.IO;
using EReader;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Bridge {
    
    public class PageTextMeshpBridge : MonoBehaviour {

        public GameObject tmpGuiFromPage(Page page) {
            GameObject pageObj = createTmpGuiObj(page.pageName + page.pageNb);
            TextMeshProUGUI tmp = pageObj.GetComponent<TextMeshProUGUI>();

            string lines = String.Join("\n", page.getContent());
            tmp.text = lines;
            return pageObj;
        }
        
        public Image imgGuiFromPage(Page page) {
            // todo
            throw new NotImplementedException();
        }

        private static GameObject createTmpGuiObj(string objName) {
            GameObject pageObj = new GameObject(objName);
            pageObj.AddComponent<TextMeshProUGUI>();
            return pageObj;
        }
    }
}
