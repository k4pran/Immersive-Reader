using System;
using UnityEditor;
using UnityEngine;

namespace Modules.VirtualBook {
    
    [CustomEditor(typeof(VirtualBook))]
    public class VirtualBookEditor : Editor {

        private int pageNumber;
        
        public override void OnInspectorGUI() {
            VirtualBook virtualBook = (VirtualBook) target;

            EditorGUILayout.LabelField("Book Info", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Title", virtualBook.title);
            EditorGUILayout.LabelField("Page Count", virtualBook.pageCount.ToString());
            
            
            EditorGUILayout.LabelField("Book Control", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Current Page", virtualBook.getDisplayableCurrentPage().ToString());

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Previous")) {
                virtualBook.Previous();
            }
            if (GUILayout.Button("Next")) {
                virtualBook.next();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            pageNumber = EditorGUILayout.IntField("Page number", pageNumber);
            if (GUILayout.Button("Go To Page")) {
                virtualBook.goTo(pageNumber);
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}