using System.Collections.Generic;
using UnityEngine;

namespace Modules.VirtualBook {
    public class VirtualPage : MonoBehaviour {
        
        public GameObject content { get; private set; }
        public bool isLeft { get; private set; }

        public static GameObject CreateVirtualPaper(Transform parentTransform, string pageName) {
            GameObject virtualPageObj = BookCreateUtils.GetPagePrefab(
                "Virtual Page - [" + pageName + "]", parentTransform);
            
            virtualPageObj.transform.parent = parentTransform;
            return virtualPageObj;
        }

        public static GameObject createBlank(Transform parentTransform) {
            GameObject virtualPageObj = BookCreateUtils.GetPagePrefab(
                "Virtual Page - [BLANK]", parentTransform);
            
            virtualPageObj.transform.parent = parentTransform;
            return virtualPageObj;
        }

        public void addContent(List<string> lines, bool isLeft=true) {
            GameObject contentPrefab = (GameObject) Resources.Load("Prefabs/PageContentTMP", typeof(GameObject));
            content = Instantiate(contentPrefab, transform);
            content.name = "content";
            content.GetComponent<PageContentTextMesh>().setText(lines);
            this.isLeft = isLeft;
        }

        public void setPositions(GameObject parent) {
            BookCreateUtils.stretchToParent(gameObject);
            BookCreateUtils.fitPageContainer(parent, content, isLeft,
                20, 20, 20, 20);
        }
    }
}
