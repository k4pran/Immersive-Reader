using System.Collections.Generic;
using System.IO;
using DefaultNamespace;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Modules.Library {
    
    public class LibraryManifest {
        
        public Dictionary<string, BookManifest> bookManifests { get; private set; }

        public LibraryManifest() {
            bookManifests = new Dictionary<string, BookManifest>();
        }

        public LibraryManifest(Dictionary<string, BookManifest> bookManifests) {
            this.bookManifests = bookManifests;
        }

        public void addEntry(BookManifest bookManifest) {
            if (bookManifest.bookID.Length == 0) {
                throw new InvalidBookIDException("Found empty ID in book manifest. This is not allowed");
            }
            bookManifests.Add(bookManifest.bookID, bookManifest);
        }

        public string serialize() {
            Serializer serializer = new SerializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .JsonCompatible()
                .Build();
            
            return serializer.Serialize(this);
        }

        public static LibraryManifest deserialize(string yaml) {
            StringReader yamlInput = new StringReader(yaml);
            Deserializer deserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();
            
            return deserializer.Deserialize<LibraryManifest>(yamlInput);
        }
    }
}