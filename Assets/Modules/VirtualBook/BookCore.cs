using System;
using Modules.Library;
using UnityEngine;
using Logger = Modules.Common.Logger;

namespace Modules.VirtualBook {

    public class BookCore : MonoBehaviour, ICoreInfo {
        
        private static readonly ILibrarian librarian;

        private void Start() {
            VirtualBasicBook.CreateVirtualBook(this).Subscribe(basicBook => {
                basicBook.gameObject.transform.parent = gameObject.transform;
            });
        }

        static BookCore() {
            ILibrary library = new VirtualFileLibrary();
            librarian = new Librarian(library);
        }

        [ReadOnly]
        public string bookId;
        
        [ReadOnly] 
        public string title;

        private bool bookIdSet;
        private bool titleSet;

        public string BookId() {
            return bookId;
        }

        public string BookTitle() {
            return title;
        }

        public void InitBookIdAs(string bookId) {
            if (bookIdSet) {
                Logger.Warning($"Ignored request to set book id {bookId}. " +
                               $"It can only be set once and is already set as {this.bookId}");
                return;
            }
            this.bookId = bookId;
            bookIdSet = true;
        }

        public void InitTitleAs(string title) {
            if (titleSet) {
                Logger.Warning($"Ignored request to set book title {title}. " +
                               $"It can only be set once and is already set as {this.title}");
                return;
            }
            this.title = title;
            titleSet = true;
        }
    }
}