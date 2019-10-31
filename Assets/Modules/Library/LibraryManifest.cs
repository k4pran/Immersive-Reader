using System.Collections.Generic;
using System.IO;
using Modules.Common;
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
        
        public List<BookToken> GetBookTokens() {
            List<BookToken> tokens = new List<BookToken>();
            Logger.Trace("Fetching book tokens");
            foreach (var keyValuePair in bookManifests) {
                Logger.Trace($"Found book token [{keyValuePair.Key} : {keyValuePair.Value}]");
                tokens.Add(new BookToken(keyValuePair.Key, keyValuePair.Value.bookTitle));
            }
            return tokens;
        }

        public void AddEntry(BookManifest bookManifest) {
            Logger.Debug("Adding book manifest to library manifest " +
                         $"[{bookManifest.bookTitle} : {bookManifest.bookId}]");
            if (bookManifest.bookId.Length == 0)
                throw new InvalidBookIdException(
                    $"Found empty ID in book manifest for book {bookManifest.bookTitle}. This is not allowed");
            bookManifests.Add(bookManifest.bookId, bookManifest);
        }

        public bool BookIdExists(string bookId) {
            return bookManifests.ContainsKey(bookId);
        }

        public BookManifest GetBookById(string bookId) {
            Logger.Trace($"Fetching book {bookId} from library manifest");
            if (BookIdExists(bookId)) return bookManifests[bookId];
            throw new BookNotFoundException("Book with id " + bookId + " not found");
        }

        public int GetBookCount() {
            return bookManifests.Count;
        }

        public string Serialize() {
            Logger.Debug("Serializing library manifest");
            var serializer = new SerializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .JsonCompatible()
                .Build();

            return serializer.Serialize(this);
        }

        public static LibraryManifest Deserialize(string yaml) {
            Logger.Debug("Deserializing library manifest");
            var yamlInput = new StringReader(yaml);
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();

            return deserializer.Deserialize<LibraryManifest>(yamlInput);
        }
    }
}