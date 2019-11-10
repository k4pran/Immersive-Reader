using System;
using Modules.Library;
using UnityEngine;
using Logger = Modules.Common.Logger;

namespace Modules.VirtualBook {

    public class TwoPageController : MonoBehaviour, IBookController {
        
        private static readonly ILibrarian librarian;
        
        [ReadOnly]
        public int pageCount;

        [LabelOverride("Current Page Number", false)]
        public int currentPageNb;

        private void Start() {
            InitPageCount();
        }

        private void InitPageCount() {
            librarian.PageCount(CoreInfo().BookId())
                .Subscribe(
                    fetchedPageCount => pageCount = fetchedPageCount,
                    error => Logger.Error(error));
        }

        static TwoPageController() {
            ILibrary library = new VirtualFileLibrary();
            librarian = new Librarian(library);
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

        private ICoreInfo CoreInfo() {
            return gameObject.transform.parent.GetComponent<BookCore>();
        }
    }
}