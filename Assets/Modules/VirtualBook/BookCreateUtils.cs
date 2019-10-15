using UnityEngine;

namespace Modules.VirtualBook {

    public class BookCreateUtils : MonoBehaviour {

        public static GameObject GetVirtualBookPrefab(string bookName, Transform parentTransform = null, float posX = 0,
            float posY = 0, float posZ = 0) {
            var virtualBookPrefab = (GameObject) Resources.Load("Prefabs/VirtualBasicBook", typeof(GameObject));
            var virtualBook = Instantiate(virtualBookPrefab, new Vector3(0, 0, 0), Quaternion.identity,
                parentTransform);
            virtualBook.name = bookName;
            return virtualBook;
        }

        public static GameObject GetPagePrefab(string pageName, Transform parentTransform, float posX = 0,
            float posY = 0, float posZ = 0) {
            GameObject pagePrefab = (GameObject) Resources.Load("Prefabs/VirtualPage", typeof(GameObject));
            GameObject page = Instantiate(pagePrefab, new Vector3(posX, posY, posZ), Quaternion.identity, parentTransform);
            page.name = pageName;
            return page;
        }

        public static void FitPageContainer(GameObject parent, GameObject pageContainer, bool isLeftPage,
            float marginTop = 0, float marginRight = 0, float marginBottom = 0, float marginLeft = 0,
            float zOffset = -30) {
            var pageRect = pageContainer.GetComponent<RectTransform>();
            var canvasHalfWidth = parent.GetComponent<RectTransform>().rect.width / 2;

            pageRect.anchorMin = new Vector2(0, 0);
            pageRect.anchorMax = new Vector2(1, 1);

            if (isLeftPage) {
                SetTop(pageContainer, marginTop);
                SetRight(pageContainer, canvasHalfWidth + marginRight);
                SetBottom(pageContainer, marginBottom);
                SetLeft(pageContainer, marginLeft);
            }
            else {
                SetTop(pageContainer, marginTop);
                SetRight(pageContainer, marginRight);
                SetBottom(pageContainer, marginBottom);
                SetLeft(pageContainer, canvasHalfWidth + marginLeft);
            }

            var currentPosition = pageContainer.transform.position;
            pageRect.transform.position = new Vector3(currentPosition.x, currentPosition.y, zOffset);
        }

        public static void SetLeft(GameObject targetObject, float left) {
            targetObject.GetComponent<RectTransform>().offsetMin =
                new Vector2(left, targetObject.GetComponent<RectTransform>().offsetMin.y);
        }

        public static void SetRight(GameObject targetObject, float right) {
            targetObject.GetComponent<RectTransform>().offsetMax =
                new Vector2(-right, targetObject.GetComponent<RectTransform>().offsetMax.y);
        }

        public static void SetTop(GameObject targetObject, float top) {
            targetObject.GetComponent<RectTransform>().offsetMax =
                new Vector2(targetObject.GetComponent<RectTransform>().offsetMax.x, -top);
        }

        public static void SetBottom(GameObject targetObject, float bottom) {
            targetObject.GetComponent<RectTransform>().offsetMin =
                new Vector2(targetObject.GetComponent<RectTransform>().offsetMin.x, bottom);
        }

//        public static void fitTMP(TextMeshProUGUI textMeshProUgui, 
//            float paddingTop, float paddingBottom, float paddingLeft, float paddingRight) {
//            RectTransform rectTransform = textMeshProUgui.GetComponent<RectTransform>();
//
//            rectTransform.offsetMax = new Vector2(-paddingLeft, -paddingTop);
//            rectTransform.offsetMin = new Vector2(paddingRight, paddingBottom);
//            rectTransform.anchorMin = new Vector2(0, 0);
//            rectTransform.anchorMax = new Vector2(1, 1);
//        }

        public static void stretchToParent(GameObject child,
            float topMargin = 0, float rightMargin = 0, float bottomMargin = 0, float leftMargin = 0) {
            var pageRect = child.GetComponent<RectTransform>();
            SetTop(child, topMargin);
            SetRight(child, rightMargin);
            SetBottom(child, bottomMargin);
            SetLeft(child, leftMargin);
            pageRect.anchorMin = new Vector2(0, 0);
            pageRect.anchorMax = new Vector2(1, 1);
        }

    }

}