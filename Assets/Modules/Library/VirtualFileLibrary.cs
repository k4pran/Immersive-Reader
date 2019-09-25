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
        
        private PhysicalFileSystem physicalFileSystem;

        private readonly string APPLICATION_NAME = "VReader";
        private readonly string ROOT_DIR_NAME = "Library";
        private readonly string LIBRARY_MANIFEST_FILENAME = "library_manifest";
        private readonly string BOOKS_LIB_DIR_NAME = "books";
        private readonly string META_DIR_NAME = "meta";
        private readonly string BOOK_META_POSTFIX = ".meta";

        private LibraryManifest libraryManifest;

        public VirtualFileLibrary() {
            physicalFileSystem = new PhysicalFileSystem();
            setup();
        }

        private void setup() {
            physicalFileSystem.CreateDirectory(getRootDir());
            physicalFileSystem.CreateDirectory(getBookLibDir());
            initializeLibrary();
        }

        private void initializeLibrary() {
            Debug.Log("Initializing library...");
            if (physicalFileSystem.FileExists(getLibraryManifestPath())) {
                Debug.Log("Library exists at " + getLibraryManifestPath());
                loadExistingLibrary();
            }
            else {
                Debug.Log("No library found. Creating now...");
                createNewLibrary();
            }
            Debug.Log("Library ready");
        }

        private void loadExistingLibrary() {
            string libraryYamlContents = readFileAsStringFromVfs(getLibraryManifestPath());
            libraryManifest = LibraryManifest.deserialize(libraryYamlContents);
        }

        private void createNewLibrary() {
            libraryManifest = new LibraryManifest();
            string emptyLibraryYaml = libraryManifest.serialize();
            saveFile(generateStreamFromString(emptyLibraryYaml), getLibraryManifestPath());
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
                catch (Exception e) {
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
                    if (physicalFileSystem.FileExists(metaInfoPath)) {
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

        private void saveLibraryManifest() {
            string libraryYaml = libraryManifest.serialize();
            Stream libraryStream = generateStreamFromString(libraryYaml);
            saveFile(libraryStream, getLibraryManifestPath());
        }

        public IObservable<BookManifest> importBook(Uri bookInputPath, BookMetaInfo bookMetaInfo) {
            return Observable.Create<BookManifest>(observer => {
                try {
                    string filename = FileUtils.getFileNameFromPath(bookInputPath.AbsolutePath);
                    string fileContents = readFileAsStringFromNative(bookInputPath);

                    Stream stream = generateStreamFromString(fileContents);
                    UPath bookDirPath = createBookDir(bookMetaInfo.title);
                    UPath bookOutputPath = Path.Combine(bookDirPath.ToString(), filename);
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

        public IObservable<byte[]> readBookAsBytes(string bookId) {
            return Observable.Create<byte[]>(observer => {
                try {
                    UPath bookUPath = libraryManifest.getBookById(bookId).bookLocation;
                    byte[] contents = readFileAsBytesFromVfs(bookUPath);
                    observer.OnNext(contents);
                    observer.OnCompleted();
                }
                catch (Exception e) {
                    observer.OnError(e);
                }

                return Disposable.Empty;
            });
        }

        public IObservable<string> readBookAsString(string bookId) {
            return Observable.Create<string>(observer => {
                try {
                    UPath bookUPath = libraryManifest.getBookById(bookId).bookLocation;
                    string contents = readFileAsStringFromVfs(bookUPath);
                    observer.OnNext(contents);
                    observer.OnCompleted();
                }
                catch (Exception e) {
                    observer.OnError(e);
                }

                return Disposable.Empty;
            });
        }
        
        public IObservable<string[]> readBookAsLines(string bookId) {
            return Observable.Create<string[]>(observer => {
                try {
                    UPath bookUPath = libraryManifest.getBookById(bookId).bookLocation;
                    string[] contents = readFileAsLinesFromVfs(bookUPath);
                    observer.OnNext(contents);
                    observer.OnCompleted();
                }
                catch (Exception e) {
                    observer.OnError(e);
                }

                return Disposable.Empty;
            });
        }

        public IObservable<Uri> retrievePhysicalFileLocation(string bookId) {
            return Observable.Create<Uri>(observer => {
                try {
                    UPath uPath = libraryManifest.bookManifests[bookId].bookLocation;
                    observer.OnNext(new Uri(uPath.ToString()));
                    observer.OnCompleted();
                }
                catch (Exception e) {
                    observer.OnError(e);
                }

                return Disposable.Empty;
            });
        }

        private UPath getRootDir() {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                APPLICATION_NAME, ROOT_DIR_NAME);
        }
        
        private UPath getLibraryManifestPath() {
            return Path.Combine(getRootDir().ToString(), LIBRARY_MANIFEST_FILENAME);
        }

        private UPath getBookLibDir() {
            return Path.Combine(getRootDir().ToString(), BOOKS_LIB_DIR_NAME);
        }

        private UPath getBookDir(string bookTitle) {
            return Path.Combine(getBookLibDir().ToString(), bookTitle);
        }
        
        private UPath getMetaInfoDir(string bookTitle) {
            return Path.Combine(getBookDir(bookTitle).ToString(), META_DIR_NAME);
        }

        private UPath createBookDir(string bookTitle) {
            UPath bookDirPath = getBookDir(bookTitle);
            physicalFileSystem.CreateDirectory(bookDirPath);
            return bookDirPath;
        }

        private UPath createMetaInfoDir(string bookTitle) {
            UPath bookDirPath = getMetaInfoDir(bookTitle);
            physicalFileSystem.CreateDirectory(bookDirPath);
            return bookDirPath;
        }
        
        private UPath saveBookMetaInfo(BookMetaInfo bookMetaInfo, string originPath) {
            UPath metaInfoDir = createMetaInfoDir(bookMetaInfo.title);
            string bookMetaYaml = bookMetaInfo.serialize();
            Stream fileContents = generateStreamFromString(bookMetaYaml);
            string filename = FileUtils.getFileNameFromPath(
                                  originPath, includeExtension: false) + BOOK_META_POSTFIX;
            UPath outputPath = Path.Combine(metaInfoDir.ToString(), filename);
            saveFile(fileContents, outputPath);
            return outputPath;
        }

        private BookManifest createBookEntry(UPath vfsBookPath, UPath bookMetaInfoPath, BookMetaInfo bookMetaInfo) {
            BookManifest bookManifest = new BookManifest(
                generateId(), bookMetaInfo.title, vfsBookPath.ToString(), bookMetaInfoPath.ToString());
            return bookManifest;
        }

        private string readFileAsStringFromVfs(UPath uPath) {
            if (physicalFileSystem.FileExists(uPath)) {
                return physicalFileSystem.ReadAllText(uPath);
            }
            throw new FileNotFoundException("File " + uPath + " not found in virtual file system");
        }
        
        private string[] readFileAsLinesFromVfs(UPath uPath) {
            if (physicalFileSystem.FileExists(uPath)) {
                return physicalFileSystem.ReadAllLines(uPath);
            }

            throw new FileNotFoundException("File " + uPath + " not found in virtual file system");
        }

        private byte[] readFileAsBytesFromVfs(UPath uPath) {
            if (physicalFileSystem.FileExists(uPath)) {
                return physicalFileSystem.ReadAllBytes(uPath);
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
            return physicalFileSystem.FileExists(uPath);
        }

        private void saveFile(Stream fileInputStream, UPath destinationPath) {
            Stream outputStream = physicalFileSystem.CreateFile(destinationPath);
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