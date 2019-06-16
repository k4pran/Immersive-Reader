using System;
using System.Collections.Generic;
using Modules.Bridge;
using UnityEngine;

namespace Modules.VirtualBook {
    public class VirtualLibrary : MonoBehaviour {

        public Dictionary<string, string> books { get; private set; }
        public Dictionary<string, VirtualBookMeta> virtualBookMetas { get; private set; }

        void Awake() {
            books = Librarian.requestAllBookIdNamePairs();
            virtualBookMetas = new Dictionary<string, VirtualBookMeta>();
            loadMetaInfo();        
        }
        
        private void loadMetaInfo() {
            foreach(KeyValuePair<string, string> book in books) {
                if (!Librarian.doesBookIdExist(book.Key)) {
                    Debug.Log("BookId " + book.Key + " does not exist and shouldn't be in the virtual library");
                }

                VirtualBookMeta virtualBookMeta = new VirtualBookMeta();
                virtualBookMeta.title = Librarian.requestTitle(book.Key);
                virtualBookMeta.author = Librarian.requestAuthor(book.Key);
                virtualBookMeta.publisher = Librarian.requestPublisher(book.Key);
                virtualBookMeta.language = Librarian.requestLanguage(book.Key);
                virtualBookMeta.description = Librarian.requestDescription(book.Key);
                virtualBookMeta.category = Librarian.requestCategory(book.Key);
                virtualBookMeta.tags = Librarian.requestTags(book.Key);
                virtualBookMeta.publicationDate = Librarian.requestPublicationDate(book.Key);
                virtualBookMeta.pageCount = Librarian.requestPageCount(book.Key);
                virtualBookMetas[book.Key] = virtualBookMeta;
            }
        }
    }
}