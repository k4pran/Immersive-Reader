using System;
using System.Collections.Generic;
using Modules.Library;
using UnityEngine;

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

        public void loadBookFromId() {
            Debug.Log("Loading book with id " + bookId + "...");
            VirtualBook.createFromId(bookId).Subscribe(
                book => {
                    Debug.Log("Book successfully loaded");
                },
                error => Debug.Log(error));
        }

        public void loadBookFromTitle() {
            VirtualBook.createFromTitle(bookTitle).Subscribe(
                book => {
                    Debug.Log("Book successfully loaded");
                },
                error => Debug.Log("ERROR"));
        }

        public void populateBooks() {
            Debug.Log("Populating books...");
            availableBooks = new List<BookInspectorElement>();
            librarian.BookCount().Subscribe(bookCount => { Debug.Log(bookCount + " books found"); });

            librarian.AvailableBooks().Subscribe(bookToken => {
                availableBooks.Add(new BookInspectorElement(bookToken.bookTitle, bookToken.bookId));
            });
        }

    }

}