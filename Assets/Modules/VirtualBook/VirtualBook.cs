using System;
using System.Collections.Generic;
using Modules.Bridge;
using UnityEngine;

namespace Modules.VirtualBook {
        
    public class VirtualBook : MonoBehaviour {

        public List<VirtualPage> virtualPages;

        [ReadOnly] 
        public string title;
        [ReadOnly] 
        public int pageCount;

        public static void createFromId(string bookId) {
            String bookName = Librarian.requestTitle(bookId);
            GameObject virtualBookObj = BookCreateUtils.GetVirtualBookPrefab(bookName);
            VirtualBook virtualBook = virtualBookObj.GetComponent<VirtualBook>();

            virtualBook.initBookInfo(bookId);
            bool isLeft = false;
            
            // todo fix number 10 below vvv
            for(int pageNum = 0; pageNum < 10; pageNum += 2) {
                GameObject virtualPageObj;
                if (pageNum >= virtualBook.pageCount) {
                    virtualPageObj = VirtualPage.CreateVirtualPaper(
                        virtualBook.transform, "Page " + pageNum, "blank");

                    List<string> lines = (List<string>) Librarian.requestPageContent(bookId, pageNum);
                    virtualPageObj.GetComponent<VirtualPage>().addContent(lines);
                    
                }
                else {
                    virtualPageObj = VirtualPage.CreateVirtualPaper(
                        virtualBook.transform, "Page " + pageNum, "Page " + (pageNum + 1));
                    
                    List<string> frontContent = (List<string>) Librarian.requestPageContent(bookId, pageNum);
                    List<string> backContent = (List<string>) Librarian.requestPageContent(bookId, pageNum + 1);

                    virtualPageObj.GetComponent<VirtualPage>().addContent(frontContent);
                    virtualPageObj.GetComponent<VirtualPage>().addContent(backContent, false);
                }

                virtualBook.appendPage(virtualPageObj.GetComponent<VirtualPage>());
                isLeft = !isLeft;
            }
            
            
        }

        private void initBookInfo(string bookId) {
            title = Librarian.requestTitle(bookId);
            initPageCount(Librarian.requestPageCount(bookId));

        }

        private void initPageCount(int pageCount) {
            this.pageCount = pageCount;
            virtualPages.Capacity = pageCount;
        }
        
        public void appendPage(VirtualPage virtualPage) {
            virtualPages.Add(virtualPage);
        }
    }
}