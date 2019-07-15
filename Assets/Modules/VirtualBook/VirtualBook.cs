using System;
using System.Collections.Generic;
using Modules.Bridge;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.VirtualBook {
        
    public class VirtualBook : MonoBehaviour {

        public List<VirtualPage> virtualPages;
        public VirtualPage currentPage;
        public int currentPageNumber;
        
        
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
                    virtualPageObj.GetComponent<VirtualPage>().setPositions(virtualBookObj);
                }
                else {
                    virtualPageObj = VirtualPage.CreateVirtualPaper(
                        virtualBook.transform, "Page " + pageNum, "Page " + (pageNum + 1));
                    
                    List<string> leftContent = (List<string>) Librarian.requestPageContent(bookId, pageNum);
                    List<string> rightContent = (List<string>) Librarian.requestPageContent(bookId, pageNum + 1);

                    virtualPageObj.GetComponent<VirtualPage>().addContent(leftContent);
                    virtualPageObj.GetComponent<VirtualPage>().addContent(rightContent, false);
                    virtualPageObj.GetComponent<VirtualPage>().setPositions(virtualBookObj);
                }
                virtualPageObj.SetActive(false);

                virtualBook.appendPage(virtualPageObj.GetComponent<VirtualPage>());
                isLeft = !isLeft;
            }
            virtualBook.setCurentPage(0);
        }

        private void setCurentPage(int pageNum) {
            if (currentPage != null) {
                currentPage.gameObject.SetActive(false);
            }
                
            if (pageNum < virtualPages.Count) {
                currentPageNumber = pageNum;
                currentPage = virtualPages[currentPageNumber];
                currentPage.gameObject.SetActive(true);
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

        public void next() {
            if (currentPageNumber == virtualPages.Count - 1) {
                return;
            }
            
            if (currentPageNumber % 2 == 0) {
                setCurentPage(currentPageNumber + 2);
            }
            else {
                setCurentPage(currentPageNumber + 1);
            }
        }

        public void Previous() {
            if (currentPageNumber <= 1) {
                setCurentPage(0);
                return;
            }
            
            if (currentPageNumber % 2 == 0) {
                setCurentPage(currentPageNumber - 1);
            }
            else {
                setCurentPage(currentPageNumber - 2);
            }
        }

        public void goTo(int pageNumber) {
            // convert to zero-indexed
            Debug.Log(pageNumber);
            pageNumber--;
            if (pageNumber < 0) {
                setCurentPage(0);
            }
            else if (pageNumber >= virtualPages.Count) {
                setCurentPage(virtualPages.Count - 1);
            }
            else {
                setCurentPage(pageNumber);
            }
        }

        public int getDisplayableCurrentPage() {
            return currentPageNumber + 1;
        }
    }
}