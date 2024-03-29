using UnityEditor;
using UnityEngine;

namespace Modules.VirtualBook {

    [CustomEditor(typeof(BookLoader))]
    public class BookLoaderEditor : Editor {

        public override void OnInspectorGUI() {
            BookLoader bookLoader = (BookLoader)target;
            EditorGUIUtility.labelWidth = 70;
            
            EditorGUILayout.BeginHorizontal();
            bookLoader.bookId = EditorGUILayout.TextField("Book ID", bookLoader.bookId);
            if (GUILayout.Button("Load from id")) {
                bookLoader.LoadBookFromId();
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space();
            
            EditorGUILayout.BeginHorizontal();
            bookLoader.bookTitle = EditorGUILayout.TextField("Book title", bookLoader.bookTitle);
            if (GUILayout.Button("Load from title")) {
                bookLoader.LoadBookFromTitle();
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space();

            
            if (GUILayout.Button("Populate books")) {
                bookLoader.PopulateBooks();
            }
            
            EditorGUILayout.PropertyField(serializedObject.FindProperty("availableBooks"), true);
        }
    }
}