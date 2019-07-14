using NUnit.Framework;
using UnityEngine;

namespace Modules.VirtualBook.Tests {
    
    public class BookCreationTests {
        
        [Test]
        public void GeneratingVirtualBookFromBookId() {
            GameObject virtualLibraryObject = new GameObject("VirtualLib");
            virtualLibraryObject.AddComponent<VirtualLibrary>();
            VirtualLibrary virtualLibrary = virtualLibraryObject.GetComponent<VirtualLibrary>();
            string bookId = virtualLibrary.randomBookId();
            VirtualBook.createFromId(bookId);
           
        }
    }
}