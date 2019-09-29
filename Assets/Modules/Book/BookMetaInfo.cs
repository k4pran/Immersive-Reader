using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Modules.Book {

    public class BookMetaInfo {

        private static readonly char COLLAPSE_DELIMITER = '|';

        public BookMetaInfo() {}

        public BookMetaInfo(string title, string author, string publisher, string language, string description,
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

        public string CollapseTags() {
            return string.Join(COLLAPSE_DELIMITER.ToString(), tags);
        }

        private List<string> SplitTags(string lines) {
            return lines.Split(COLLAPSE_DELIMITER).ToList();
        }

        public string Serialize() {
            var serializer = new SerializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .JsonCompatible()
                .Build();

            return serializer.Serialize(this);
        }

        public static BookMetaInfo Deserialize(string yaml) {
            var yamlInput = new StringReader(yaml);
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();

            return deserializer.Deserialize<BookMetaInfo>(yamlInput);
        }
    }
}