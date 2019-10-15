using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using TMPro;
using UnityEngine;

namespace Modules.VirtualBook {

    public class TextContent : IPageContent {

        public static IObservable<GameObject> tmpGuiFromText(string text) {
            return Observable.Create<GameObject>(observable => {
                GameObject gameObject = new GameObject();
                TextMeshProUGUI textMesh = gameObject.AddComponent<TextMeshProUGUI>();
                textMesh.fontSizeMin = 8;
                textMesh.fontSizeMax = 200;
                textMesh.color = Color.black;
                textMesh.enableAutoSizing = true;

                textMesh.text = text;
                observable.OnNext(gameObject);
                observable.OnCompleted();
                return Disposable.Empty;
            });
        }
        
        public static IObservable<GameObject> tmpGuiFromText(string[] lines) {
            return tmpGuiFromText(string.Join("\n", lines));
        }
    }
}