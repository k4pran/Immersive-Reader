using System;
using TMPro;
using UnityEngine;

namespace Modules.VirtualBook {
    public class BookCreateUtils : MonoBehaviour {
        public static GameObject GetVirtualBookPrefab(string bookName, Transform parentTransform=null, float posX=0, float posY=0, float posZ=0) {
            GameObject virtualBookPrefab = (GameObject) Resources.Load("Prefabs/VirtualBook", typeof(GameObject));
            GameObject virtualBook = Instantiate(virtualBookPrefab, new Vector3(0, 0, 0), Quaternion.identity, parentTransform);
            virtualBook.name = bookName;
            return virtualBook;
        }       
        
        public static GameObject GetPagePrefab(string pageName, Transform parentTransform, float posX=0, float posY=0, float posZ=0){
            GameObject pagePrefab = (GameObject) Resources.Load("Prefabs/VirtualPage", typeof(GameObject));
            GameObject page = Instantiate(pagePrefab, new Vector3(posX, posY, posZ), Quaternion.identity, parentTransform);
            page.name = pageName;
            return page;
        }
        
        public static void fitPageContainer(GameObject canvas, GameObject pageContainer, Boolean isLeftPage){
        
            RectTransform pageRect = pageContainer.GetComponent<RectTransform>();
            float canvasHalfWidth = canvas.GetComponent<RectTransform>().rect.width / 2;
        
            pageRect.anchorMin = new Vector2(0, 0);
            pageRect.anchorMax = new Vector2(1, 1);

            if (isLeftPage) {
                SetLeft(pageContainer, 0);
                SetRight(pageContainer, canvasHalfWidth);
                SetTop(pageContainer, 0);
                SetBottom(pageContainer, 0);
            }
            else {
                SetLeft(pageContainer, canvasHalfWidth);
                SetRight(pageContainer, 0);
                SetTop(pageContainer, 0);
                SetBottom(pageContainer, 0);
            }
        }
        
        public static void SetLeft(GameObject targetObject, float left) {
            targetObject.GetComponent<RectTransform>().offsetMin = new Vector2(left, targetObject.GetComponent<RectTransform>().offsetMin.y);
        }

        public static void SetRight(GameObject targetObject, float right) {
            targetObject.GetComponent<RectTransform>().offsetMax = new Vector2(-right, targetObject.GetComponent<RectTransform>().offsetMax.y);
        }

        public static void SetTop(GameObject targetObject, float top) {
            targetObject.GetComponent<RectTransform>().offsetMax = new Vector2(targetObject.GetComponent<RectTransform>().offsetMax.x, -top);
        }

        public static void SetBottom(GameObject targetObject, float bottom) {
            targetObject.GetComponent<RectTransform>().offsetMin = new Vector2(targetObject.GetComponent<RectTransform>().offsetMin.x, bottom);
        }

        public static void fitTMP(TextMeshProUGUI textMeshProUgui, 
            float paddingTop, float paddingBottom, float paddingLeft, float paddingRight) {
            RectTransform rectTransform = textMeshProUgui.GetComponent<RectTransform>();

            rectTransform.offsetMax = new Vector2(-paddingLeft, -paddingTop);
            rectTransform.offsetMin = new Vector2(paddingRight, paddingBottom);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
        }

        public static void stretchToParent(GameObject child) {
            RectTransform pageRect = child.GetComponent<RectTransform>();
            pageRect.offsetMax = new Vector2(0, 0);
            pageRect.offsetMin = new Vector2(0, 0);
            pageRect.anchorMin = new Vector2(0, 0);
            pageRect.anchorMax = new Vector2(1, 1);
        }
    }
}