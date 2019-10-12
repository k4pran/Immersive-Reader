using System;

namespace Modules.VirtualBook {

    [Serializable]
    public struct BookInspectorElement {
        public string title;
        public string id;

        public BookInspectorElement(string title, string id) {
            this.title = title;
            this.id = id;
        }
    }
}