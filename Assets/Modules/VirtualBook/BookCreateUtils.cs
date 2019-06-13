using UnityEngine;

namespace VirtualBook {
    public class BookCreateUtils : MonoBehaviour {
        
        public static GameObject GetPagePrefab(string pageName, Transform parentTransform, float posX=0, float posY=0, float posZ=0){
            GameObject pagePrefab = (GameObject)Resources.Load("prefabs/PageBlank", typeof(GameObject));
            GameObject page = Instantiate(pagePrefab, new Vector3(posX, posY, posZ), Quaternion.identity, parentTransform);
            page.name = pageName;
            return page;
        }
    }
}