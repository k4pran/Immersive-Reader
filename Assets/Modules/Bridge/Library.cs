using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using EReader;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Bridge {
    
    public class Library {
        
        private static Library instance;
        private static readonly object padlock = new object();

        public List<Shelf> shelves { get; set; }
        public Dictionary<string, Book> books { get; set; }
                
        public Library() {
            if (instance != null){
                throw new NotSupportedException("Only one instance of Library is allowed. Access via Library.Instance");
            }
        }

        public static Library Instance {
            get {
                lock(padlock) {
                    if (instance == null) {
                        initialize();
                        Deserialize();
                    }
                    return instance;
                }
            }
        }

        private static void initialize() {
            string libraryPath = Config.Instance.libraryPath;
            if (!File.Exists(libraryPath)) {
                Library library = new Library();
                library.shelves = new List<Shelf>();
                library.books = new Dictionary<string, Book>();
                library.serialize();
            }
        }
        
        public void serialize() {
            var serializer = new SerializerBuilder()
                .WithTagMapping("!basicBook", typeof(BasicBook))
                .WithTagMapping("!basicPage", typeof(BasicPage))
                .EmitDefaults()
                .DisableAliases()
                .Build();            
            var yaml = serializer.Serialize(Instance);
            File.WriteAllText(Config.Instance.libraryPath, yaml);
        }

        private static void Deserialize() {
            
            string libraryContent;
            FileStream fileStream = new FileStream(Config.Instance.libraryPath, FileMode.Open, FileAccess.Read);
            using (StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8)) {
                libraryContent = streamReader.ReadToEnd();
            }
            
            StringReader yamlInput = new StringReader(libraryContent);
            Deserializer deserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .WithTagMapping("!basicBook", typeof(BasicBook))
                .WithTagMapping("!basicPage", typeof(BasicPage))
                .Build();

            instance = deserializer.Deserialize<Library>(yamlInput);
        }

        public Book retrieveBook(string bookId) {
            if (!doesLibraryContain(bookId)) {
                throw new BookNotFoundException("Unable to find book with id " + bookId);
            }
            return books[bookId];
        }

        public bool doesLibraryContain(string bookId) {
            return books.ContainsKey(bookId);
        }

        public void addShelf(Shelf shelf) {
            shelves.Add(shelf);
        }
        
        public void addBook(Book book) {
            books.Add(book.bookId, book);
        }
    }

    class BookNotFoundException : Exception {
        
        public BookNotFoundException(){}

        public BookNotFoundException(string message): base(message){}
    }
}