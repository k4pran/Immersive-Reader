using System.IO;
using UnityEngine;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Modules.Library {
    
    public class BookManifest {
        
        public string bookID { get; private set; }
        public string bookTitle { get; private set; }
        public string bookLocation { get; private set; }
        public string metaInfoLocation { get; private set; }

        public BookManifest() {}

        public BookManifest(string bookId, string bookTitle, string bookLocation, string metaInfoLocation) {
            bookID = bookId;
            this.bookTitle = bookTitle;
            this.bookLocation = bookLocation;
            this.metaInfoLocation = metaInfoLocation;
        }

        public string Serialize() {
            Debug.Log("Serializing book manifest: [" + bookTitle + " : " + bookID + "]");
            Serializer serializer = new SerializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .JsonCompatible()
                .Build();
            
            return serializer.Serialize(this);
        }

        public static BookManifest deserialize(string yaml) {
            StringReader yamlInput = new StringReader(yaml);
            Deserializer deserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();
            
            return deserializer.Deserialize<BookManifest>(yamlInput);
        }
    }
}