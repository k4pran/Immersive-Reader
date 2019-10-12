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

        public static GameObject CreateBlank(Transform parentTransform) {
            var virtualPageObj = BookCreateUtils.GetPagePrefab(
                "Virtual Page - [BLANK]", parentTransform);

            virtualPageObj.transform.parent = parentTransform;
            return virtualPageObj;
        }

        public IPageContent AddContent(GameObject contentPrefab, bool isLeft = true) {
            content = Instantiate(contentPrefab, transform);
            content.name = "content";
            this.isLeft = isLeft;
            return content.GetComponent<IPageContent>();
        }

        public void SetPositions(GameObject parent) {
            BookCreateUtils.stretchToParent(content);
            BookCreateUtils.fitPageContainer(parent, gameObject, isLeft);
        }
    }
}