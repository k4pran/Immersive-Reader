using UnityEngine;

namespace Modules.VirtualBook.Tests {
    public class TestPdfBook : MonoBehaviour {

        public void Start() {
            GameObject virtualLibraryObject = new GameObject("VirtualLib");
            
            virtualLibraryObject.AddComponent<VirtualLibrary>();
            VirtualLibrary virtualLibrary = virtualLibraryObject.GetComponent<VirtualLibrary>();
            VirtualBook.createFromTitle("Atari");
        }
    }
}