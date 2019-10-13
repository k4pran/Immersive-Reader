using System;
using System.Collections.Generic;
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
            Debug.Log("Loading book with id " + bookId + "...");
            VirtualBook.CreateFromId(bookId).Subscribe(
                book => {
                    Debug.Log("Book successfully loaded");
                },
                error => Debug.Log(error));
        }

        public void LoadBookFromTitle() {
            VirtualBook.CreateFromTitle(bookTitle)
                .Subscribe(
                book => {
                    Debug.Log("Book successfully loaded");
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

    }

}