using Modules.Bridge;
using UnityEngine;

namespace Modules.VirtualBook {
        
    public class VirtualBookEntity : MonoBehaviour {
        
        private BookPro bookPro;
        public int pageCount { get; private set; }

        VirtualBookEntity(string bookId) {
            Librarian.requestPageCount(bookId);
        }

        public void appendPage(Paper paper) {
            Paper[] currentPapers = bookPro.papers;
            if (bookPro.papers.Length - 1 <= pageCount) {
                Paper[] papers = new Paper[pageCount * 2];
                for(int i = 0; i < pageCount; i++) {
                    papers[i] = currentPapers[i];
                }
                bookPro.papers = papers;
            }
            bookPro.papers[pageCount] = paper;
            pageCount++;
        } 
    }
}