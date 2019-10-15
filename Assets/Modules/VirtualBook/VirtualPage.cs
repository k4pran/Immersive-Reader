using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using UnityEngine;

namespace Modules.VirtualBook {

    public class VirtualPage : MonoBehaviour {
        public bool isLeft;

        public static IObservable<GameObject> CreateVirtualPaper(Transform parentTransform, string pageName) {
            return Observable.Create<GameObject>(observer => {
                try {
                    GameObject virtualPageObj = BookCreateUtils.GetPagePrefab(
                        "Virtual Page - [" + pageName + "]", parentTransform);

                    virtualPageObj.transform.parent = parentTransform;
                    observer.OnNext(virtualPageObj);
                }
                catch (Exception e) {
                    observer.OnError(e);
                }
                
                return Disposable.Empty;
            });
        }

        public static GameObject CreateBlank(Transform parentTransform) {
            var virtualPageObj = BookCreateUtils.GetPagePrefab(
                "Virtual Page - [BLANK]", parentTransform);

            virtualPageObj.transform.parent = parentTransform;
            return virtualPageObj;
        }

        public void AddContent(GameObject contentGameObj, bool isLeft = true) {
            contentGameObj.transform.parent = gameObject.transform;
            contentGameObj.name = "content";
            
            BookCreateUtils.stretchToParent(contentGameObj, 20, 20, 20, 20);
            BookCreateUtils.FitPageContainer(transform.parent.gameObject, gameObject, isLeft);
            this.isLeft = isLeft;
        }
    }
}