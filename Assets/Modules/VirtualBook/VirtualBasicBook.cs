using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Modules.Library;
using UnityEngine;
using Logger = Modules.Common.Logger;

namespace Modules.VirtualBook {
        
    public class VirtualBasicBook : MonoBehaviour {

        private static readonly ILibrarian librarian;

        public List<VirtualPage> virtualPages;

        [ReadOnly]
        private const int PAGE_BUFFER_SIZE = 12;
        

        private void Start() {
            virtualPages = new List<VirtualPage>();
            InitPages();
        }

        private void InitPages() {
            CreatePages(gameObject.GetComponent<TwoPageController>().CurrentPageNb() - PAGE_BUFFER_SIZE / 2, 
                PAGE_BUFFER_SIZE / 2).Subscribe(
                fetchedPage => {
                    virtualPages.Add(fetchedPage); 
                }, 
                error => Logger.Error(error));
        }
        
        static VirtualBasicBook() {
            ILibrary library = new VirtualFileLibrary();
            librarian = new Librarian(library);
        }

        public static IObservable<VirtualBasicBook> CreateVirtualBook(BookCore seed) {
            return Observable.Return(seed)
                .Select(bookCore => BookCreateUtils.GetVirtualBookPrefab("Basic Book"))
                .Select(virtualBookPrefab => virtualBookPrefab.GetComponent<VirtualBasicBook>());
        }

        public IObservable<VirtualPage[]> CreatePages() {
            return librarian.PageCount(CoreInfo().BookId())
                .Select(pageCount => Enumerable.Range(0, pageCount))
                .SelectMany(pageNb => pageNb)
                .Select(CreatePage)
                .Concat()
                .ToArray();
        }

        private IObservable<VirtualPage> CreatePages(int startPageNb, int endPageNb) {
            return librarian.PageCount(CoreInfo().BookId())
                .Select(fetchedPageCount => {
                    startPageNb = startPageNb < 0 ? 0 : startPageNb;
                    endPageNb = endPageNb > gameObject.GetComponent<TwoPageController>().pageCount ? 
                        gameObject.GetComponent<TwoPageController>().pageCount : endPageNb;
                    return Enumerable.Range(startPageNb, endPageNb);
                })
                .SelectMany(pageNb => pageNb)
                .SelectMany(CreatePage);
        }

        private IObservable<VirtualPage> CreatePage(int pageNb) {
            return VirtualPage.CreateVirtualPaper(gameObject.transform, pageNb.ToString())
                .Select(pageGameObject => pageGameObject.GetComponent<VirtualPage>())
                .Zip(CreateTmpGui(CoreInfo().BookId(), pageNb), (virtualPage, textMeshGui) => {
                    bool isLeft = pageNb % 2 == 0;
                    virtualPage.AddContent(textMeshGui, isLeft);
                    return virtualPage;
                });
        }

        private static IObservable<GameObject> CreateTmpGui(string bookId, int pageNb) {
            return librarian.PageContents<List<string>>(bookId, pageNb)
                .Select(pageLines => string.Join("\n", pageLines))
                .Select(TextContent.tmpGuiFromText)
                .Concat();
        }
        
        private ICoreInfo CoreInfo() {
            return gameObject.transform.parent.GetComponent<BookCore>();
        }
    }
}

