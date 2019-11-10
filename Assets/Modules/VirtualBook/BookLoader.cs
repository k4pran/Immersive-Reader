using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Modules.Library;
using UnityEngine;
using Logger = Modules.Common.Logger;

namespace Modules.VirtualBook {

    public class BookLoader : MonoBehaviour {

        private ILibrarian librarian;

        public string bookId { get; set; }
        public string bookTitle { get; set; }


        [ReadOnly] [SerializeField] public List<BookInspectorElement> availableBooks;

        private void Reset() {
            ILibrary virtualFileLibrary = new VirtualFileLibrary();
            librarian = new Librarian(virtualFileLibrary);
            availableBooks = new List<BookInspectorElement>();
        }

        public void LoadBookFromId() {
            Debug.Log("Loading book with id " + bookId);
            librarian.Title(bookId).Select(derivedTitle => createBookCore(bookId, derivedTitle));
        }

        public void LoadBookFromTitle() {
            Debug.Log("Loading book with title " + bookTitle);
            librarian.BookIdByTitle(bookTitle)
                .Select(derivedBookId => createBookCore(derivedBookId, bookTitle))
                .Subscribe(book => {
                    Logger.Info("Book created");
                },
                    error => Logger.Error(error));
        }

        public void PopulateBooks() {
            Debug.Log("Populating books...");
            availableBooks = new List<BookInspectorElement>();
            librarian.BookCount().Subscribe(bookCount => { Debug.Log(bookCount + " books found"); });

            librarian.AvailableBooks().Subscribe(bookToken => {
                availableBooks.Add(new BookInspectorElement(bookToken.bookTitle, bookToken.bookId));
            });
        }

        private BookCore createBookCore(string bookId, string title) {
            BookCore bookCore = new GameObject($"{title}").gameObject.AddComponent<BookCore>();
            bookCore.InitBookIdAs(bookId);
            bookCore.InitTitleAs(title);
            return bookCore;
        }
    }
}