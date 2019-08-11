﻿using System;
using System.Collections.Generic;
using Modules.Bridge;
 using Modules.Common;
 using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Modules.VirtualBook {
        
    public class VirtualBook : MonoBehaviour {

        [FormerlySerializedAs("virtualPage")] public List<VirtualPage> virtualPages;
        [FormerlySerializedAs("currentPage")] public VirtualPage currentPage;
        public int currentPageNumber;
        
        
        [ReadOnly] 
        public string title;
        [ReadOnly] 
        public int pageCount;

        public static void createFromTitle(string bookTitle) {
            createFromId(Librarian.requestId(bookTitle));
        }

        public static void createFromId(string bookId) {
            String bookName = Librarian.requestTitle(bookId);
            GameObject virtualBookObj = BookCreateUtils.GetVirtualBookPrefab(bookName);
            VirtualBook virtualBook = virtualBookObj.GetComponent<VirtualBook>();
            ContentType contentType = Librarian.requestContentType(bookId);

            switch(contentType) {
                
                case ContentType.TEXT_ONLY:
                    createAsTextContent(virtualBook, virtualBookObj, bookId);
                    break;
                
                case ContentType.IMAGE:
                    createAsImageContent(virtualBook, virtualBookObj, bookId);
                    break;
                
                default:
                    throw new ContentTypeException("Content type " + contentType + " not recognised");
                
            }
            
            virtualBook.initBookInfo(bookId);
        }

        private static void createAsTextContent(VirtualBook virtualBook, GameObject virtualBookObj, string bookId) {
            bool isLeft = true;
            
            // todo fix number 10 below vvv
            for(int pageNum = 0; pageNum < 11; pageNum += 1) {
                GameObject virtualPageObj;

                virtualPageObj = VirtualPage.CreateVirtualPaper(
                    virtualBook.transform, "Page " + pageNum);
                
                List<string> lines = (List<string>) Librarian.requestPageContent(bookId, pageNum);
                
                GameObject contentPrefab = (GameObject) Resources.Load("Prefabs/PageContentTMP", typeof(GameObject));


                PageContentTextMesh pageContent = (PageContentTextMesh) virtualPageObj.GetComponent<VirtualPage>().addContent(contentPrefab, isLeft);
                pageContent.setText(lines);
                virtualPageObj.GetComponent<VirtualPage>().setPositions(virtualBookObj);
                virtualPageObj.SetActive(false);

                virtualBook.appendPage(virtualPageObj.GetComponent<VirtualPage>());
                isLeft = !isLeft;
            }

            if (virtualBook.virtualPages.Count % 2 == 1) {
                GameObject blank = VirtualPage.createBlank(virtualBook.transform);
                GameObject contentPrefab = (GameObject) Resources.Load("Prefabs/PageContentTMP", typeof(GameObject));
                blank.GetComponent<VirtualPage>().addContent(contentPrefab, false);
                blank.GetComponent<VirtualPage>().setPositions(virtualBookObj);
                blank.SetActive(false);
                virtualBook.appendPage(blank.GetComponent<VirtualPage>());
            }
            
            virtualBook.setCurrentPage(0);
        }

        private static void createAsImageContent(VirtualBook virtualBook, GameObject virtualBookObj, string bookId) {
  
        }

        private void setCurrentPage(int pageNum) {
            if (currentPage != null) {
                currentPage.gameObject.SetActive(false);
                setSiblingPage(false);
            }
                
            if (pageNum < virtualPages.Count) {
                currentPageNumber = pageNum;
                currentPage = virtualPages[currentPageNumber];
                currentPage.gameObject.SetActive(true);
            }
            setSiblingPage(true);
        }

        private void setSiblingPage(bool active) {
            if (currentPage.isLeft) {
                virtualPages[currentPageNumber + 1].gameObject.SetActive(active);
            }
            else {
                virtualPages[currentPageNumber - 1].gameObject.SetActive(active);
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
            int targetPageumber = currentPage.isLeft ? currentPageNumber + 1 : currentPageNumber;
            if (targetPageumber >= virtualPages.Count - 1) {
                setCurrentPage(virtualPages.Count - 1);
                return;
            }
            setCurrentPage(targetPageumber + 1);
        }

        public void Previous() {
            int targetPageumber = currentPage.isLeft ? currentPageNumber : currentPageNumber - 1;
            if (targetPageumber <= 1) {
                setCurrentPage(0);
                return;
            }
            setCurrentPage(targetPageumber - 1);
        }

        public void goTo(int pageNumber) {
            // convert to zero-indexed
            Debug.Log(pageNumber);
            pageNumber--;
            if (pageNumber < 0) {
                setCurrentPage(0);
            }
            else if (pageNumber >= virtualPages.Count) {
                setCurrentPage(virtualPages.Count - 1);
            }
            else {
                setCurrentPage(pageNumber);
            }
        }

        public int getDisplayableCurrentPage() {
            return currentPageNumber + 1;
        }
    }
}