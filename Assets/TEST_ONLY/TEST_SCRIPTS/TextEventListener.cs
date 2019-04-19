using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TextEventListener : MonoBehaviour {
    
    [Serializable]
    public class CharacterHoverEvent : UnityEvent<char, int> {}

    [Serializable]
    public class SpriteHoverEvent : UnityEvent<char, int> {}

    [Serializable]
    public class WordHoverEvent : UnityEvent<string, int, int> {}

    [Serializable]
    public class LineHoverEvent : UnityEvent<string, int, int> {}

    [Serializable]
    public class LinkHoverEvent : UnityEvent<string, string, int> {}
    
    [Serializable]
    public class WordClickEvent : UnityEvent<TMP_Text, TMP_WordInfo, int> {}


    /// <summary>
    /// Event delegate triggered when pointer is over a character.
    /// </summary>
    public CharacterHoverEvent OnCharacterHover {
        get { return _mOnCharacterHover; }
        set { _mOnCharacterHover = value; }
    }

    [SerializeField] private CharacterHoverEvent _mOnCharacterHover = new CharacterHoverEvent();


    /// <summary>
    /// Event delegate triggered when pointer is over a sprite.
    /// </summary>
    public SpriteHoverEvent OnSpriteHover {
        get { return _mOnSpriteHover; }
        set { _mOnSpriteHover = value; }
    }

    [SerializeField] private SpriteHoverEvent _mOnSpriteHover = new SpriteHoverEvent();


    /// <summary>
    /// Event delegate triggered when pointer is over a word.
    /// </summary>
    public WordHoverEvent onWordHover {
        get { return _mOnWordHover; }
        set { _mOnWordHover = value; }
    }

    [SerializeField] private WordHoverEvent _mOnWordHover = new WordHoverEvent();


    /// <summary>
    /// Event delegate triggered when pointer is over a line.
    /// </summary>
    public LineHoverEvent OnLineHover {
        get { return _mOnLineHover; }
        set { _mOnLineHover = value; }
    }

    [SerializeField] private LineHoverEvent _mOnLineHover = new LineHoverEvent();


    /// <summary>
    /// Event delegate triggered when pointer is over a link.
    /// </summary>
    public LinkHoverEvent OnLinkHover {
        get { return _mOnLinkHover; }
        set { _mOnLinkHover = value; }
    }

    [SerializeField] private LinkHoverEvent _mOnLinkHover = new LinkHoverEvent();
    
    /// <summary>
    /// Event delegate triggered when click on a word.
    /// </summary>
    public WordClickEvent onWordClick
    {
        get { return _mOnWordClick; }
        set { _mOnWordClick = value; }
    }
    [SerializeField]
    private WordClickEvent _mOnWordClick= new WordClickEvent();


    private TMP_Text m_TextComponent;

    private Camera m_Camera;
    private Canvas m_Canvas;

    private int m_selectedLink = -1;
    private int m_lastCharIndex = -1;
    private int m_lastWordIndex = -1;
    private int m_lastLineIndex = -1;

    void Awake() {
        // Get a reference to the text component.
        m_TextComponent = gameObject.GetComponent<TMP_Text>();

        // Get a reference to the camera rendering the text taking into consideration the text component type.
        if (m_TextComponent.GetType() == typeof(TextMeshProUGUI)) {
            m_Canvas = gameObject.GetComponentInParent<Canvas>();
            if (m_Canvas != null) {
                if (m_Canvas.renderMode == RenderMode.ScreenSpaceOverlay)
                    m_Camera = null;
                else
                    m_Camera = m_Canvas.worldCamera;
            }
        }
        else {
            m_Camera = Camera.main;
        }
    }


    void LateUpdate() {
        if (TMP_TextUtilities.IsIntersectingRectTransform(m_TextComponent.rectTransform, Input.mousePosition, m_Camera)) {
            #region Example of Character or Sprite Selection

            int charIndex =
                TMP_TextUtilities.FindIntersectingCharacter(m_TextComponent, Input.mousePosition, m_Camera, true);
            if (charIndex != -1 && charIndex != m_lastCharIndex) {
                m_lastCharIndex = charIndex;

                TMP_TextElementType elementType = m_TextComponent.textInfo.characterInfo[charIndex].elementType;

                // Send event to any event listeners depending on whether it is a character or sprite.
                if (elementType == TMP_TextElementType.Character)
                    SendOnCharacterSelection(m_TextComponent.textInfo.characterInfo[charIndex].character, charIndex);
                else if (elementType == TMP_TextElementType.Sprite)
                    SendOnSpriteSelection(m_TextComponent.textInfo.characterInfo[charIndex].character, charIndex);
            }

            #endregion


            #region Example of Word Selection

            int wordIndex = TMP_TextUtilities.FindIntersectingWord(m_TextComponent, Input.mousePosition, m_Camera);
            if (wordIndex != -1 && wordIndex != m_lastWordIndex) {
                m_lastWordIndex = wordIndex;

                // Get the information about the selected word.
                TMP_WordInfo wInfo = m_TextComponent.textInfo.wordInfo[wordIndex];

                // Send the event to any listeners.
                SendOnWordSelection(wInfo.GetWord(), wInfo.firstCharacterIndex, wInfo.characterCount);
            }
            
            if (wordIndex != -1 && Input.GetMouseButtonDown(0)) {

                TMP_WordInfo wInfo = m_TextComponent.textInfo.wordInfo[wordIndex];
                SendOnWordClick(m_TextComponent, wInfo, wordIndex);
            }

            #endregion


            #region Example of Line Selection

            int lineIndex = TMP_TextUtilities.FindIntersectingLine(m_TextComponent, Input.mousePosition, m_Camera);
            if (lineIndex != -1 && lineIndex != m_lastLineIndex) {
                m_lastLineIndex = lineIndex;

                // Get the information about the selected word.
                TMP_LineInfo lineInfo = m_TextComponent.textInfo.lineInfo[lineIndex];

                // Send the event to any listeners.
                char[] buffer = new char[lineInfo.characterCount];
                for(int i = 0; i < lineInfo.characterCount && i < m_TextComponent.textInfo.characterInfo.Length; i++) {
                    buffer[i] = m_TextComponent.textInfo.characterInfo[i + lineInfo.firstCharacterIndex].character;
                }

                string lineText = new string(buffer);
                SendOnLineSelection(lineText, lineInfo.firstCharacterIndex, lineInfo.characterCount);
            }

            #endregion


            #region Example of Link Handling

            // Check if mouse intersects with any links.
            int linkIndex = TMP_TextUtilities.FindIntersectingLink(m_TextComponent, Input.mousePosition, m_Camera);

            // Handle new Link selection.
            if (linkIndex != -1 && linkIndex != m_selectedLink) {
                m_selectedLink = linkIndex;

                // Get information about the link.
                TMP_LinkInfo linkInfo = m_TextComponent.textInfo.linkInfo[linkIndex];

                // Send the event to any listeners. 
                SendOnLinkSelection(linkInfo.GetLinkID(), linkInfo.GetLinkText(), linkIndex);
            }

            #endregion
        }
    }


    public void OnPointerEnter(PointerEventData eventData) {
        //Debug.Log("OnPointerEnter()");
    }


    public void OnPointerExit(PointerEventData eventData) {
        //Debug.Log("OnPointerExit()");
    }


    private void SendOnCharacterSelection(char character, int characterIndex) {
        if (OnCharacterHover != null)
            OnCharacterHover.Invoke(character, characterIndex);
    }

    private void SendOnSpriteSelection(char character, int characterIndex) {
        if (OnSpriteHover != null)
            OnSpriteHover.Invoke(character, characterIndex);
    }

    private void SendOnWordSelection(string word, int charIndex, int length) {
        if (onWordHover != null)
            onWordHover.Invoke(word, charIndex, length);
    }

    private void SendOnLineSelection(string line, int charIndex, int length) {
        if (OnLineHover != null)
            OnLineHover.Invoke(line, charIndex, length);
    }

    private void SendOnLinkSelection(string linkID, string linkText, int linkIndex) {
        if (OnLinkHover != null)
            OnLinkHover.Invoke(linkID, linkText, linkIndex);
    }
    
    private void SendOnWordClick(TMP_Text tmp, TMP_WordInfo wordInfo, int wordIndex) {
        if (onWordClick != null)
            onWordClick.Invoke(tmp, wordInfo, wordIndex);
    }
}