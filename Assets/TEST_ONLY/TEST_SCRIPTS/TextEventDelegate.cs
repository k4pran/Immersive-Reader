using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextEventDelegate : MonoBehaviour {

    public TextEventListener textEventListener;

    void OnEnable() {
        if (textEventListener != null) {
            textEventListener.onWordHover.AddListener(onWordHover);
            textEventListener.onWordClick.AddListener(onWordClick);
        }
    }


    void OnDisable() {
        if (textEventListener != null) {
            textEventListener.onWordHover.RemoveListener(onWordHover);
            textEventListener.onWordClick.RemoveListener(onWordClick);
        }
    }
    
    
    void onWordHover(string word, int firstCharacterIndex, int length) {
        Debug.Log("Word [" + word + "] with first character index of " + firstCharacterIndex + " and length of " + length + " has been hovered.");
    }    
    
    void onWordClick(TMP_Text tmp, TMP_WordInfo wordInfo, int wordIndex) {
        Debug.Log("Word [" + wordInfo.GetWord() + "] with first character index of " + wordInfo.firstCharacterIndex 
                  + " and length of " +  wordInfo.characterCount + " has been selected.");
    }   
}
