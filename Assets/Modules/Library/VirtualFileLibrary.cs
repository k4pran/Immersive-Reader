using System;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Modules.Book;
using Modules.Common;
using UnityEngine;
using Zio;
using Zio.FileSystems;

namespace Modules.Library {
    
    public class VirtualFileLibrary : ILibrary {

        private MemoryFileSystem memoryFileSystem;

        private readonly string ROOT_DIR = "/VReader Library";
        private readonly string LIBRARY_MANIFEST_FILENAME = "library";
        private readonly string BOOK_DIR = "books";
        private readonly string BOOK_META_DIR = "books";
        private readonly string BOOK_META_POSTFIX = ".meta";
        private readonly UPath libraryManifestPath;

        private LibraryManifest libraryManifest;
            
        public VirtualFileLibrary() {
            memoryFileSystem = new MemoryFileSystem();
            libraryManifestPath = new UPath(Path.Combine(ROOT_DIR, LIBRARY_MANIFEST_FILENAME));
            setup();
        }

        private void setup() {
            memoryFileSystem.CreateDirectory(ROOT_DIR);
            memoryFileSystem.CreateDirectory(Path.Combine(ROOT_DIR, BOOK_DIR));
            initializeLibrary();
        }

        private void initializeLibrary() {
            Debug.Log("Initializing library...");
            if (memoryFileSystem.FileExists(libraryManifestPath)) {
                Debug.Log("Library exists at " + libraryManifestPath);
                loadExistingLibrary();
            }
            else {
                Debug.Log("No library found. Creating now...");
                createNewLibrary();
            }
            Debug.Log("Library ready");
        }

        private void loadExistingLibrary() {
            string libraryYamlContents = readFileAsStringFromVfs(libraryManifestPath);
            libraryManifest = LibraryManifest.deserialize(libraryYamlContents);
        }

        private void createNewLibrary() {
            libraryManifest = new LibraryManifest();
            string emptyLibraryYaml = libraryManifest.serialize();
            saveFile(generateStreamFromString(emptyLibraryYaml), libraryManifestPath);
        }

        public IObservable<LibraryManifest> retrieveLibraryManifest() {
            return Observable.Create<LibraryManifest>(observer => {
                observer.OnNext(libraryManifest);
                observer.OnCompleted();
                return Disposable.Empty;
            });
        }
        
        public IObservable<BookManifest> retrieveBookManifest(string bookId) {
            return Observable.Create<BookManifest>(observer => {
                try {
                    BookManifest bookManifest = libraryManifest.getBookById(bookId);
                    observer.OnNext(bookManifest);
                    observer.OnCompleted();
                }
                catch(Exception e) {
                    observer.OnError(e);
                }
                return Disposable.Empty;
            });
        }
        
        public IObservable<BookMetaInfo> retrieveBookMetaInfo(string bookId) {
            return Observable.Create<BookMetaInfo>(observer => {
                try {
                    string metaInfoPath = libraryManifest.getBookById(bookId).metaInfoLocation;
                    UPath uMetaInfoPath = asUpath(metaInfoPath);
                    if (memoryFileSystem.FileExists(metaInfoPath)) {
                        String metaInfoYaml = readFileAsStringFromVfs(uMetaInfoPath);
                        BookMetaInfo bookMetaInfo = BookMetaInfo.deserialize(metaInfoYaml);
                        observer.OnNext(bookMetaInfo);
                        observer.OnCompleted();
                    }
                    else {
                        observer.OnError(new BookNotFoundException("Unable to find book with id " + bookId));
                    }
                }
                catch (Exception e) {
                    observer.OnError(e);
                }
                return Disposable.Empty;
            });
        }
        
        public IObservable<int> getBookCount() {
            return Observable.Create<int>(observer => {
                observer.OnNext(libraryManifest.getBookCount());
                observer.OnCompleted();
                return Disposable.Empty;
            });
        }

        private void addBookToLibraryManifest(BookManifest bookManifest) {
             libraryManifest.addEntry(bookManifest);
        }

        public void saveLibraryManifest() {
            foreach (BookManifest bookManifest in libraryManifest.bookManifests.Values) {
            UPath uBookPath = asUpath(bookManifest.bookLocation);
                if (!bookExists(uBookPath)) {
                    Debug.Log("Saving book to library [" +
                              bookManifest.bookId + " : " + bookManifest.bookTitle);
                    string contents = bookManifest.Serialize();
                    saveFile(generateStreamFromString(contents), uBookPath);
                }
            }
        }

        public IObservable<BookManifest> importBook(Uri bookInputPath, BookMetaInfo bookMetaInfo) {
            return Observable.Create<BookManifest>(observer => {
                try {
                    string filename = FileUtils.getFileNameFromPath(bookInputPath.AbsolutePath);
                    string fileContents = readFileAsStringFromNative(bookInputPath);
                    
                    Stream stream = generateStreamFromString(fileContents);
                    UPath bookOutputPath = Path.Combine(ROOT_DIR, BOOK_DIR, filename);
                    saveFile(stream, bookOutputPath);
                    UPath metaInfoLocation = saveBookMetaInfo(bookMetaInfo, bookInputPath.AbsolutePath);
                    
                    BookManifest bookManifest = createBookEntry(bookOutputPath, metaInfoLocation, bookMetaInfo);
                    addBookToLibraryManifest(bookManifest);
                    saveLibraryManifest();
                    observer.OnNext(bookManifest);
                    observer.OnCompleted();
                }
                catch (Exception e) {
                    observer.OnError(e);
                }
                return Disposable.Empty;
            });
        }
        
        public UPath saveBookMetaInfo(BookMetaInfo bookMetaInfo, string originPath) {
            string bookMetaYaml = bookMetaInfo.serialize();
            Stream fileContents = generateStreamFromString(bookMetaYaml);
            string filename = FileUtils.getFileNameFromPath(
                originPath, includeExtension: false) + BOOK_META_POSTFIX;
            UPath outputPath = Path.Combine(ROOT_DIR, BOOK_META_DIR, filename);
            saveFile(fileContents, outputPath);
            return outputPath;
        }

        private BookManifest createBookEntry(UPath vfsBookPath, UPath bookMetaInfoPath, BookMetaInfo bookMetaInfo) {
            BookManifest bookManifest = new BookManifest(
                generateId(), bookMetaInfo.title, vfsBookPath.ToString(), bookMetaInfoPath.ToString());
            return bookManifest;
        }

        private string readFileAsStringFromVfs(UPath uPath) {
            if (memoryFileSystem.FileExists(uPath)) {
                return memoryFileSystem.ReadAllText(uPath);
            }
            throw new FileNotFoundException("File " + uPath + " not found in virtual file system");
        }

        private string readFileAsStringFromNative(Uri path) {
            if (path.IsFile && File.Exists(path.AbsolutePath)) {
                return File.ReadAllText(path.AbsolutePath);
            }
            throw new FileNotFoundException("File " + path + " not found in native file system");
        }

        private bool bookExists(UPath uPath) {
            return memoryFileSystem.FileExists(uPath);
        }
        
        private void saveFile(Stream fileInputStream, UPath destinationPath) {
            Stream outputStream = memoryFileSystem.CreateFile(destinationPath);
            fileInputStream.CopyTo(outputStream);
        }

        private UPath asUpath(string path) {
            return new UPath(path);
        }
        
        public string generateId() {
            return Guid.NewGuid().ToString("D");
        }
        
        private static Stream generateStreamFromString(string s) {
            Stream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}