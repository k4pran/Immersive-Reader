using System.IO;
using Modules.Common;
using UnityEngine;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Modules.Library {

    public class BookManifest {

        public BookManifest() {}

        public BookManifest(string bookId, string bookTitle, string bookLocation, string originalLocation,
            string metaInfoLocation, ContentType contentType, FileType fileType) {
            this.bookId = bookId;
            this.bookTitle = bookTitle;
            this.bookLocation = bookLocation;
            this.originalLocation = originalLocation;
            this.metaInfoLocation = metaInfoLocation;
            this.contentType = contentType;
            this.fileType = fileType;
        }

        public string bookId { get; }
        public string bookTitle { get; }
        public string bookLocation { get; }
        public string originalLocation { get; }
        public string metaInfoLocation { get; }
        public ContentType contentType { get; }
        public FileType fileType { get; }

        public string Serialize() {
            Debug.Log("Serializing book manifest: [" + bookTitle + " : " + bookId + "]");
            var serializer = new SerializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .JsonCompatible()
                .Build();

            return serializer.Serialize(this);
        }

        public static BookManifest Deserialize(string yaml) {
            var yamlInput = new StringReader(yaml);
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();

            return deserializer.Deserialize<BookManifest>(yamlInput);
        }
    }
}