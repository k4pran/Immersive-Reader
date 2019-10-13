using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Modules.Library;
using TMPro;
using UnityEngine;
using Logger = Modules.Common.Logger;

namespace Modules.VirtualBook {
        
    public class VirtualBook : MonoBehaviour, IBookController {

        private static ILibrarian librarian;

        public List<VirtualPage> virtualPages;

        [ReadOnly]
        public string bookId;
        
        [ReadOnly] 
        public string title;

        [ReadOnly]
        public int pageCount;

        [LabelOverride("Current Page Number", false)]
        public int currentPageNb;

        void Start() {
            InitPageCount();
            InitTitle();
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
        
        static VirtualBook() {
            ILibrary library = new VirtualFileLibrary();
            librarian = new Librarian(library);
        }
        
        public static IObservable<VirtualBook> CreateFromId(string bookId) {
            return CreateVirtualBook(bookId)
                .Do(book => book.bookId = bookId);
        }
        
        public static IObservable<VirtualBook> CreateFromTitle(string title) {
            return librarian.BookIdByTitle(title)
                .Select(bookId => CreateFromId(bookId))
                .Concat();
        }

        public static IObservable<VirtualBook> CreateVirtualBook(string bookId) {
            return librarian.Title(bookId)
                .Select(title => BookCreateUtils.GetVirtualBookPrefab(title))
                .Select(virtualBookPrefab => virtualBookPrefab.GetComponent<VirtualBook>());
        }

        public IObservable<VirtualPage[]> CreatePages() {
            return librarian.PageCount(bookId)
                .Select(pageCount => Enumerable.Range(0, pageCount))
                .SelectMany(pageNb => pageNb)
                .Select(pageNb => CreatePage(bookId, pageNb))
                .Concat()
                .ToArray();
        }

        public IObservable<VirtualPage> CreatePage(string bookId, int pageNb) {
            return librarian.PageContents<string>(bookId, pageNb)
                .Select(text => TextContent.tmpGuiFromText(text))
                .Select(tmpGui => VirtualPage.CreateVirtualPaper(null, pageNb.ToString()))
                .Select(pageGameObject => pageGameObject.GetComponent<VirtualPage>())
                .Zip(CreateTmpGui(bookId, pageNb), (virtualPage, textMeshGui) => {
                    bool isLeft = pageNb % 2 == 1;
                    virtualPage.AddContent(textMeshGui.gameObject, isLeft);
                    return virtualPage;
                });
        }

        public IObservable<TextMeshProUGUI> CreateTmpGui(string bookId, int pageNb) {
            return librarian.PageContents<string>(bookId, pageNb)
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

