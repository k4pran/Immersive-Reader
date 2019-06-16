using NUnit.Framework;
using UnityEngine;

namespace Modules.VirtualBook.Tests {

    public class InitializationTests : MonoBehaviour {   

        [Test]
        public void LibraryGetsInitializedWithBookIdsAndMetaInfo() {
            GameObject virtualLibraryObject = new GameObject("virtualLib");
            virtualLibraryObject.AddComponent<VirtualLibrary>();
            VirtualLibrary virtualLibrary = virtualLibraryObject.GetComponent<VirtualLibrary>();
            Assert.NotNull(virtualLibrary.books);
            Assert.NotNull(virtualLibrary.virtualBookMetas);
        }
    }
}
