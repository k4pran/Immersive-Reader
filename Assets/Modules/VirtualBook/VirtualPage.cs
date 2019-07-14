using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Modules.VirtualBook {
    public class VirtualPage : MonoBehaviour {

        public PageContent frontContent;
        public PageContent backContent;
        
        public static GameObject CreateVirtualPaper(Transform parentTransform, string frontSideName, string backSideName) {
            GameObject virtualPageObj = BookCreateUtils.GetPagePrefab(
                "Virtual Page - [" + frontSideName + " - " + backSideName + "]", parentTransform);
            
            virtualPageObj.transform.parent = parentTransform;
            return virtualPageObj;
        }

        public void addContent(List<string> lines, bool isFrontContent=true) {
            GameObject contentPrefab = (GameObject) Resources.Load("Prefabs/PageContentTMP", typeof(GameObject));
            GameObject pageContentTextMeshObj = Instantiate(contentPrefab);
            PageContentTextMesh pageContentTextMesh = pageContentTextMeshObj.GetComponent<PageContentTextMesh>();
            pageContentTextMesh.setText(lines);
            if (isFrontContent) {
                frontContent = pageContentTextMesh;
                contentPrefab.transform.parent = frontContent.transform.parent;

            }
            else {
                backContent = pageContentTextMesh;
                contentPrefab.transform.parent = backContent.transform.parent;
               
            }
        }
    }
}
