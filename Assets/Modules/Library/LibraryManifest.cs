using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Modules.Library {

    public class LibraryManifest {

        public LibraryManifest() {
            bookManifests = new Dictionary<string, BookManifest>();
        }

        public LibraryManifest(Dictionary<string, BookManifest> bookManifests) {
            this.bookManifests = bookManifests;
        }

        public Dictionary<string, BookManifest> bookManifests { get; }

        public List<BookToken> GetBookTokens() {
            var tokens = new List<BookToken>();
            foreach (var keyValuePair in bookManifests)
                tokens.Add(new BookToken(keyValuePair.Key, keyValuePair.Value.bookTitle));

            return tokens;
        }

        public void AddEntry(BookManifest bookManifest) {
            if (bookManifest.bookId.Length == 0)
                throw new InvalidBookIdException("Found empty ID in book manifest. This is not allowed");
            bookManifests.Add(bookManifest.bookId, bookManifest);
        }

        public bool BookIdExists(string bookId) {
            return bookManifests.ContainsKey(bookId);
        }

        public BookManifest GetBookById(string bookId) {
            if (BookIdExists(bookId)) return bookManifests[bookId];
            throw new BookNotFoundException("Book with id " + bookId + " not found");
        }

        public int GetBookCount() {
            return bookManifests.Count;
        }

        public string Serialize() {
            var serializer = new SerializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .JsonCompatible()
                .Build();

            return serializer.Serialize(this);
        }

        public static LibraryManifest Deserialize(string yaml) {
            var yamlInput = new StringReader(yaml);
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();

            return deserializer.Deserialize<LibraryManifest>(yamlInput);
        }
    }
}