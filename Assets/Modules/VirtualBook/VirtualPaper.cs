using TMPro;
using UnityEngine;
using VirtualBook;

public class VirtualPaper {

    public Paper paper;

    public VirtualPaper(VirtualBookEntity bookEntity, Paper paper, string frontPageName, string backPageName) {
        this.paper = paper;
        if (this.paper.Front == null) BookCreateUtils.GetPagePrefab(frontPageName, bookEntity.transform);
        if (this.paper.Back == null) BookCreateUtils.GetPagePrefab(backPageName, bookEntity.transform);
    }
    
    public VirtualPaper(VirtualBookEntity bookEntity, Paper paper, string frontPageName, string backPageName, 
            GameObject frontContent=null, GameObject backContent=null) {
        this.paper = paper;
        if (this.paper.Front == null) BookCreateUtils.GetPagePrefab(frontPageName, bookEntity.transform);
        if (this.paper.Back == null) BookCreateUtils.GetPagePrefab(backPageName, bookEntity.transform);

        if (frontContent != null) addFrontContent(frontContent);
        if (backContent != null) addBackContent(backContent);
    }
    
    public VirtualPaper(VirtualBookEntity bookEntity, Paper paper, string frontPageName, string backPageName,
            TextMeshProUGUI frontContent=null, TextMeshProUGUI backContent=null) {
        this.paper = paper;
        if (this.paper.Front == null) BookCreateUtils.GetPagePrefab(frontPageName, bookEntity.transform);
        if (this.paper.Back == null) BookCreateUtils.GetPagePrefab(backPageName, bookEntity.transform);
        
        if (frontContent != null) addFrontContent(frontContent);
        if (backContent != null) addBackContent(backContent);
    }

    public void addFrontContent(GameObject content) {
        addContent(paper.Front.transform, content.gameObject);
    }

    public void addBackContent(GameObject content) {
        addContent(paper.Back.transform, content.gameObject);
    }
    
    public void addFrontContent(TextMeshProUGUI content) {
        addContent(paper.Front.transform, content.gameObject);
    }

    public void addBackContent(TextMeshProUGUI content) {
        addContent(paper.Back.transform, content.gameObject);
    }

    private void addContent(Transform parentTransform, GameObject content) {
        content.transform.parent = parentTransform;
    }
}
