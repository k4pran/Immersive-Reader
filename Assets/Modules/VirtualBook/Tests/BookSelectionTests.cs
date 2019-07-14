using NUnit.Framework;
using UnityEngine;

namespace Modules.VirtualBook.Tests {
    public class BookSelectionTests : MonoBehaviour {

        [Test]
        public void selectingABookInitializedVirtualBookEntity() {
            GameObject virtualLibraryObj = new GameObject("VirtualLib");
            virtualLibraryObj.AddComponent<VirtualLibrary>();
            VirtualLibrary virtualLibrary = virtualLibraryObj.GetComponent<VirtualLibrary>();
            string bookId = virtualLibrary.randomBookId();
            GameObject virtualBookEntity = new GameObject();
            virtualBookEntity.AddComponent<VirtualBook>();
        }
    }
}