using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Modules.Library;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Modules.VirtualBook {
        
    public class VirtualBook : MonoBehaviour {

        private static ILibrarian librarian;

        [FormerlySerializedAs("virtualPage")]
        public List<VirtualPage> virtualPages;
        [FormerlySerializedAs("currentPage")] public VirtualPage currentPage;
        public int currentPageNumber;

        public string bookId;
        
        [ReadOnly] 
        public string title;
        [ReadOnly] 
        public int pageCount;

        static VirtualBook() {
            ILibrary library = new VirtualFileLibrary();
            librarian = new Librarian(library);
        }
        
        public static IObservable<VirtualBook> createFromId(string bookId) {
            return createVirtualBook(bookId);
        }
        
        public static IObservable<VirtualBook> createFromTitle(string title) {
            return librarian.BookIdByTitle(title)
                .Select(bookId => createVirtualBook(bookId))
                .Concat();
        }

        public static IObservable<VirtualBook> createVirtualBook(string bookId) {
            return librarian.Title(bookId)
                .Select(title => BookCreateUtils.GetVirtualBookPrefab(title))
                .Select(virtualBookPrefab => virtualBookPrefab.GetComponent<VirtualBook>());
        }

        public static IObservable<VirtualPage[]> createPages(string bookId) {
            return librarian.PageCount(bookId)
                .Select(pageCount => Enumerable.Range(0, pageCount))
                .SelectMany(pageNb => pageNb)
                .Select(pageNb => createPage(bookId, pageNb))
                .Concat()
                .ToArray();
        }

        public static IObservable<VirtualPage> createPage(string bookId, int pageNb) {
            return librarian.PageContents<string>(bookId, pageNb)
                .Select(text => TextContent.tmpGuiFromText(text))
                .Select(tmpGui => VirtualPage.CreateVirtualPaper(null, pageNb.ToString()))
                .Select(pageGameObject => pageGameObject.GetComponent<VirtualPage>())
                .Zip(createTmpGui(bookId, pageNb), (virtualPage, textMeshGui) => {
                    bool isLeft = pageNb % 2 == 1;
                    virtualPage.AddContent(textMeshGui.gameObject, isLeft);
                    return virtualPage;
                });
        }

        public static IObservable<TextMeshProUGUI> createTmpGui(string bookId, int pageNb) {
            return librarian.PageContents<string>(bookId, pageNb)
                .Select(text => TextContent.tmpGuiFromText(text))
                .Concat();
        }
    }
}

