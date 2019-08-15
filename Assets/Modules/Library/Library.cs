using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Modules.Book.Tests.Book;
using UnityEngine;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Modules.Library {
    
    public class Library {
        
        private static Library instance;
        private static readonly object padlock = new object();

        public List<Shelf> shelves { get; set; }
        public SqlConn sqlConn;
        public string currentBookId { get; set; }
                
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

        public void init() {
            Debug.Log("Library initialization requested...");
        }

        private static void initialize() {
            string libraryPath = Config.Instance.libraryPath;
            if (!File.Exists(libraryPath)) {
                Library library = new Library();
                library.shelves = new List<Shelf>();
                library.sqlConn = new SqlConn();
                library.serialize();
            }
        }
        
        public void serialize() {
            var serializer = new SerializerBuilder()
                .WithTagMapping("!basicBook", typeof(BasicBook))
                .WithTagMapping("!basicPage", typeof(TextPage))
                .WithTagMapping("!pdfBasicBook", typeof(PdfBasicBook))
                .WithTagMapping("!imagePage", typeof(ImagePage))
                .EmitDefaults()
                .DisableAliases()
                .Build();   
            
            var yaml = serializer.Serialize(this);
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
                .WithTagMapping("!basicPage", typeof(TextPage))
                .WithTagMapping("!pdfBasicBook", typeof(PdfBasicBook))
                .WithTagMapping("!imagePage", typeof(ImagePage))
                .Build();

            instance = deserializer.Deserialize<Library>(yamlInput);
        }

        public List<object> retrieveAllBooks() {
            throw new NotImplementedException();
        }

        public Book<T> retrieveBook<T>(string bookId) {
            if (!doesLibraryContainId(bookId)) {
                throw new BookNotFoundException("Unable to find book with id " + bookId);
            }
            throw new NotImplementedException();
        }

        public BookFormat retrieveBookFormat(string bookId) {
            throw new NotImplementedException();
        }

        public bool doesLibraryContainId(string bookId) {
            throw new NotImplementedException();
        }
        
        public bool doesLibraryContainTitle(string title) {
            throw new NotImplementedException();
        }

        public void addShelf(Shelf shelf) {
            shelves.Add(shelf);
            serialize();
        }
        
        public void addBook(BasicBook book) {
            sqlConn.insertIntoBooks(book);
            serialize();
        }
        
        public void addBook(PdfBasicBook book) {
            sqlConn.insertIntoBooks(book);
            serialize();
        }
    }

    class BookNotFoundException : Exception {
        
        public BookNotFoundException(){}

        public BookNotFoundException(string message): base(message){}
    }
}