using System.Collections.Generic;
using System.IO;
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
            if (bookManifest.bookId.Length == 0) {
                throw new InvalidBookIdException("Found empty ID in book manifest. This is not allowed");
            }
            bookManifests.Add(bookManifest.bookId, bookManifest);
        }

        public bool bookIdExists(string bookId) {
            return bookManifests.ContainsKey(bookId);
        }

        public BookManifest getBookById(string bookId) {
            if (bookIdExists(bookId)) {
                return bookManifests[bookId];
            }
            throw new BookNotFoundException("Book with id " + bookId + " not found");
        }

        public int getBookCount() {
            return bookManifests.Count;
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