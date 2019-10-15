using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Modules.Library;
using TMPro;
using UnityEngine;
using Logger = Modules.Common.Logger;

namespace Modules.VirtualBook {
        
    public class VirtualBasicBook : MonoBehaviour, IBookController {

        private static ILibrarian librarian;

        public List<VirtualPage> virtualPages;
        
        [ReadOnly]
        public int PAGE_BUFFER_SIZE = 12;

        [ReadOnly]
        public string bookId;
        
        [ReadOnly] 
        public string title;

        [ReadOnly]
        public int pageCount;

        [LabelOverride("Current Page Number", false)]
        public int currentPageNb;

        void Start() {
            virtualPages = new List<VirtualPage>();
            InitPageCount();
            InitTitle();
            InitPages();
        }

        void Update() {
        }

        private void InitPageCount() {
            librarian.PageCount(bookId)
                .Subscribe(
                    fetchedPageCount => pageCount = fetchedPageCount,
                    error => Logger.Error(error));
        }
        
        private void InitTitle() {
            librarian.Title(bookId)
                .Subscribe(
                    fetchedTitle => title = fetchedTitle,
                    error => Logger.Error(error));
        }

        private void InitPages() {
            CreatePages(CurrentPageNb() - PAGE_BUFFER_SIZE / 2, PAGE_BUFFER_SIZE / 2).Subscribe(
                fetchedPage => {
                    virtualPages.Add(fetchedPage); 
                }, 
                error => Logger.Error(error));
        }
        
        static VirtualBasicBook() {
            ILibrary library = new VirtualFileLibrary();
            librarian = new Librarian(library);
        }
        
        public static IObservable<VirtualBasicBook> CreateFromId(string bookId) {
            return CreateVirtualBook(bookId)
                .Do(book => book.bookId = bookId);
        }
        
        public static IObservable<VirtualBasicBook> CreateFromTitle(string title) {
            return librarian.BookIdByTitle(title)
                .Select(bookId => CreateFromId(bookId))
                .Concat();
        }

        public static IObservable<VirtualBasicBook> CreateVirtualBook(string bookId) {
            return librarian.Title(bookId)
                .Select(title => BookCreateUtils.GetVirtualBookPrefab(title))
                .Select(virtualBookPrefab => virtualBookPrefab.GetComponent<VirtualBasicBook>());
        }

        public IObservable<VirtualPage[]> CreatePages() {
            return librarian.PageCount(bookId)
                .Select(pageCount => Enumerable.Range(0, pageCount))
                .SelectMany(pageNb => pageNb)
                .Select(pageNb => CreatePage(pageNb))
                .Concat()
                .ToArray();
        }
        
        public IObservable<VirtualPage> CreatePages(int startPageNb, int endPageNb) {
            return librarian.PageCount(bookId)
                .Select(fetchedPageCount => {
                    startPageNb = startPageNb < 0 ? 0 : startPageNb;
                    endPageNb = endPageNb > pageCount ? pageCount : endPageNb;
                    return Enumerable.Range(startPageNb, endPageNb);
                })
                .SelectMany(pageNb => pageNb)
                .SelectMany(pageNb => CreatePage(pageNb));
        }

        public IObservable<VirtualPage> CreatePage(int pageNb) {
            return VirtualPage.CreateVirtualPaper(gameObject.transform, pageNb.ToString())
                .Select(pageGameObject => pageGameObject.GetComponent<VirtualPage>())
                .Zip(CreateTmpGui(bookId, pageNb), (virtualPage, textMeshGui) => {
                    bool isLeft = pageNb % 2 == 0;
                    virtualPage.AddContent(textMeshGui, isLeft);
                    return virtualPage;
                });
        }

        public IObservable<GameObject> CreateTmpGui(string bookId, int pageNb) {
            return librarian.PageContents<List<string>>(bookId, pageNb)
                .Select(pageLines => String.Join("\n", pageLines))
                .Select(text => TextContent.tmpGuiFromText(text))
                .Concat();
        }

        public int CurrentPageNb() {
            return currentPageNb;
        }

        public int Next() {
            if (currentPageNb >= GoToEnd() - 2) {
                currentPageNb = GoToEnd();
                return CurrentPageNb();
            }

            currentPageNb += 2;
            return CurrentPageNb();
        }

        public int Previous() {
            if (currentPageNb <= 1) {
                currentPageNb = GoToStart();
                return CurrentPageNb();
            }

            currentPageNb -= 2;
            return CurrentPageNb();
        }

        public int GoTo(int pageNb) {
            if (pageNb < GoToStart()) {
                currentPageNb = GoToStart();
                return CurrentPageNb();
            }

            if (pageNb > GoToEnd()) {
                currentPageNb = GoToEnd();
                return CurrentPageNb();
            }

            currentPageNb = pageNb;
            return CurrentPageNb();
        }

        public int GoToStart() {
            currentPageNb = 0;
            return CurrentPageNb();
        }

        public int GoToEnd() {
            currentPageNb = pageCount - 1;
            return CurrentPageNb();
        }
    }
}

