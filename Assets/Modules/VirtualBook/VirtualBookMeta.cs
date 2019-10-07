using System;

namespace Modules.VirtualBook {

    public class VirtualBookMeta {

        public VirtualBookMeta() {
        }

        public VirtualBookMeta(string title, string author, string publisher, string language, string description,
            string category, string[] tags, DateTime publicationDate, int pageCount) {
            this.title = title;
            this.author = author;
            this.publisher = publisher;
            this.language = language;
            this.description = description;
            this.category = category;
            this.tags = tags;
            this.publicationDate = publicationDate;
            this.pageCount = pageCount;
        }

        public string title { get; set; }
        public string author { get; set; }
        public string publisher { get; set; }
        public string language { get; set; }
        public string description { get; set; }
        public string category { get; set; }
        public string[] tags { get; set; }
        public DateTime publicationDate { get; set; }
        public int pageCount { get; set; }

    }

}