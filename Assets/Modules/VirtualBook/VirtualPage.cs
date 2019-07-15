using System.Collections.Generic;
using UnityEngine;

namespace Modules.VirtualBook {
    public class VirtualPage : MonoBehaviour {
        
        private GameObject left;
        private GameObject right;

        private string LEFT_NAME = "Left Content";
        private string RIGHT_NAME = "Right Content";

        public static GameObject CreateVirtualPaper(Transform parentTransform, string frontSideName, string backSideName) {
            GameObject virtualPageObj = BookCreateUtils.GetPagePrefab(
                "Virtual Page - [" + frontSideName + " - " + backSideName + "]", parentTransform);
            
            virtualPageObj.transform.parent = parentTransform;
            return virtualPageObj;
        }

        public void addContent(List<string> lines, bool isLeftContent=true) {
            GameObject contentPrefab = (GameObject) Resources.Load("Prefabs/PageContentTMP", typeof(GameObject));
            GameObject content = Instantiate(contentPrefab, transform);
            content.name = isLeftContent ? LEFT_NAME : RIGHT_NAME;
            content.GetComponent<PageContentTextMesh>().setText(lines);

            if (isLeftContent) {
                left = content;
            }
            else {
                right = content;
            }
        }

        public void setPositions(GameObject parent) {
            BookCreateUtils.stretchToParent(gameObject);
            BookCreateUtils.fitPageContainer(parent, left, true);
            BookCreateUtils.fitPageContainer(parent, right, false);
        }
    }
}
