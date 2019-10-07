using System;
using System.Collections.Generic;
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

        private readonly string applicationName = "VReader";
        private readonly string bookMetaPostfix = ".meta";
        private readonly string booksLibDirName = "books";
        private readonly string libraryManifestFilename = "library_manifest";
        private readonly string metaDirName = "meta";
        private readonly string pagePrefix = "Page";
        private readonly string pagesDirName = "pages";

        private readonly PhysicalFileSystem physicalFileSystem;
        private readonly string rootDirName = "Library";

        private LibraryManifest libraryManifest;

        public VirtualFileLibrary() {
            physicalFileSystem = new PhysicalFileSystem();
            Setup();
        }

        public IObservable<LibraryManifest> RetrieveLibraryManifest() {
            return Observable.Create<LibraryManifest>(observer => {
                observer.OnNext(libraryManifest);
                observer.OnCompleted();
                return Disposable.Empty;
            });
        }

        public IObservable<IBook<Page>> ReadBookAsObject(string bookId) {
            return Observable.Create<IBook<Page>>(observer => {
                try {
                    var book = CreateBookWithFactory(bookId);
                    observer.OnNext(book);
                }
                catch (Exception e) {
                    observer.OnError(e);
                }

                return Disposable.Empty;
            });
        }

        public IObservable<BookManifest> RetrieveBookManifest(string bookId) {
            return Observable.Create<BookManifest>(observer => {
                try {
                    var bookManifest = libraryManifest.GetBookById(bookId);
                    observer.OnNext(bookManifest);
                    observer.OnCompleted();
                }
                catch (Exception e) {
                    observer.OnError(e);
                }

                return Disposable.Empty;
            });
        }

        public IObservable<BookMetaInfo> RetrieveBookMetaInfo(string bookId) {
            return Observable.Create<BookMetaInfo>(observer => {
                try {
                    ReadBookMetaInfoFromVfs(bookId);
                }
                catch (Exception e) {
                    observer.OnError(e);
                }

                return Disposable.Empty;
            });
        }

        public IObservable<int> GetBookCount() {
            return Observable.Create<int>(observer => {
                observer.OnNext(libraryManifest.GetBookCount());
                observer.OnCompleted();
                return Disposable.Empty;
            });
        }

        public IObservable<BookManifest> ImportBook(Uri bookInputPath, BookMetaInfo bookMetaInfo,
            ContentType contentType, params KeyValuePair<Option, object>[] options) {
            return Observable.Create<BookManifest>(observer => {
                try {
                    // Save book
                    var filename = FileUtils.FileNameFromPath(bookInputPath.AbsolutePath);
                    var bookDirPath = CreateBookDir(bookMetaInfo.title);
                    var bookOutputPath = new Uri(Path.Combine(bookDirPath.ToString(), filename));
                    SaveBook(bookInputPath, bookOutputPath);

                    // Save meta info
                    var metaInfoLocation = SaveBookMetaInfo(bookMetaInfo, bookInputPath.AbsolutePath);

                    // Create book entry and add to library
                    var bookManifest = CreateBookEntry(bookOutputPath.AbsolutePath,
                        bookInputPath, metaInfoLocation, bookMetaInfo, contentType);
                    AddBookToLibraryManifest(bookManifest);
                    SaveLibraryManifest();

                    // Index book
                    IndexBook(bookManifest.bookId, options);

                    // Done
                    observer.OnNext(bookManifest);
                    observer.OnCompleted();
                }
                catch (Exception e) {
                    observer.OnError(e);
                }

                return Disposable.Empty;
            });
        }

        public IObservable<byte[]> ReadBookAsBytes(string bookId) {
            return Observable.Create<byte[]>(observer => {
                try {
                    UPath bookUPath = libraryManifest.GetBookById(bookId).bookLocation;
                    var contents = ReadFileAsBytesFromVfs(bookUPath);
                    observer.OnNext(contents);
                    observer.OnCompleted();
                }
                catch (Exception e) {
                    observer.OnError(e);
                }

                return Disposable.Empty;
            });
        }

        public IObservable<string> ReadBookAsString(string bookId) {
            return Observable.Create<string>(observer => {
                try {
                    UPath bookUPath = libraryManifest.GetBookById(bookId).bookLocation;
                    var contents = ReadFileAsStringFromVfs(bookUPath);
                    observer.OnNext(contents);
                    observer.OnCompleted();
                }
                catch (Exception e) {
                    observer.OnError(e);
                }

                return Disposable.Empty;
            });
        }

        public IObservable<string[]> ReadBookAsLines(string bookId) {
            return Observable.Create<string[]>(observer => {
                try {
                    UPath bookUPath = libraryManifest.GetBookById(bookId).bookLocation;
                    var contents = ReadFileAsLinesFromVfs(bookUPath);
                    observer.OnNext(contents);
                    observer.OnCompleted();
                }
                catch (Exception e) {
                    observer.OnError(e);
                }

                return Disposable.Empty;
            });
        }

        public IObservable<string> ReadPageAsString(string bookId, int pageNb) {
            return Observable.Create<string>(observer => {
                try {
                    var bookTitle = libraryManifest.GetBookById(bookId).bookTitle;
                    var fileType = libraryManifest.GetBookById(bookId).fileType;
                    var pagePath = GetPagePath(bookTitle, pageNb, fileType);

                    var contents = ReadFileAsStringFromVfs(pagePath);
                    observer.OnNext(contents);
                    observer.OnCompleted();
                }
                catch (Exception e) {
                    observer.OnError(e);
                }

                return Disposable.Empty;
            });
        }

        public IObservable<string[]> ReadPageAsLines(string bookId, int pageNb) {
            return Observable.Create<string[]>(observer => {
                try {
                    var bookTitle = libraryManifest.GetBookById(bookId).bookTitle;
                    var fileType = libraryManifest.GetBookById(bookId).fileType;
                    var pagePath = GetPagePath(bookTitle, pageNb, fileType);

                    var contents = ReadFileAsLinesFromVfs(pagePath);
                    observer.OnNext(contents);
                    observer.OnCompleted();
                }
                catch (Exception e) {
                    observer.OnError(e);
                }

                return Disposable.Empty;
            });
        }

        public IObservable<byte[]> ReadPageAsBytes(string bookId, int pageNb) {
            return Observable.Create<byte[]>(observer => {
                try {
                    var bookTitle = libraryManifest.GetBookById(bookId).bookTitle;
                    var fileType = libraryManifest.GetBookById(bookId).fileType;
                    var pagePath = GetPagePath(bookTitle, pageNb, fileType);

                    var contents = ReadFileAsBytesFromVfs(pagePath);
                    observer.OnNext(contents);
                    observer.OnCompleted();
                }
                catch (Exception e) {
                    observer.OnError(e);
                }

                return Disposable.Empty;
            });
        }

        public IObservable<Uri> RetrievePhysicalFileLocation(string bookId) {
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

        private void Setup() {
            physicalFileSystem.CreateDirectory(GetRootDir());
            physicalFileSystem.CreateDirectory(GetBookLibDir());
            InitializeLibrary();
        }

        private void InitializeLibrary() {
            Debug.Log("Initializing library...");
            if (physicalFileSystem.FileExists(GetLibraryManifestPath())) {
                Debug.Log("Library exists at " + GetLibraryManifestPath());
                LoadExistingLibrary();
            }
            else {
                Debug.Log("No library found. Creating now...");
                CreateNewLibrary();
            }

            Debug.Log("Library ready");
        }

        private void LoadExistingLibrary() {
            var libraryYamlContents = ReadFileAsStringFromVfs(GetLibraryManifestPath());
            libraryManifest = LibraryManifest.Deserialize(libraryYamlContents);
        }

        private void CreateNewLibrary() {
            libraryManifest = new LibraryManifest();
            var emptyLibraryYaml = libraryManifest.Serialize();
            SaveFile(GenerateStreamFromString(emptyLibraryYaml), GetLibraryManifestPath());
        }

        private void AddBookToLibraryManifest(BookManifest bookManifest) {
            libraryManifest.AddEntry(bookManifest);
        }

        private void SaveLibraryManifest() {
            var libraryYaml = libraryManifest.Serialize();
            var libraryStream = GenerateStreamFromString(libraryYaml);
            SaveFile(libraryStream, GetLibraryManifestPath());
        }

        private void IndexBook(string bookId, params KeyValuePair<Option, object>[] options) {
            var bookManifest = libraryManifest.GetBookById(bookId);
            var bookLocation = new Uri(bookManifest.bookLocation);
            var pagesDir = new Uri(CreatePagesDir(bookManifest.bookTitle).ToString());
            var bookTitle = bookManifest.bookTitle;
            var contentType = bookManifest.contentType;

            Indexer.index(bookLocation, pagesDir, bookTitle, contentType, options);
        }

        private void SaveBook(Uri inputPath, Uri outputPath) {
            var fileContents = ReadFileAsStringFromNative(inputPath);

            var stream = GenerateStreamFromString(fileContents);
            UPath bookOutputPath = Path.Combine(outputPath.AbsolutePath);
            SaveFile(stream, bookOutputPath);
        }

        private IBook<Page> CreateBookWithFactory(string bookId) {
            var contentType = libraryManifest.GetBookById(bookId).contentType;
            UPath bookPath = libraryManifest.GetBookById(bookId).bookLocation;
            var bookMetaInfo = ReadBookMetaInfoFromVfs(bookId);

            switch (contentType) {
                case ContentType.TEXT_ONLY:
                    var content = ReadFileAsLinesFromVfs(bookPath);
                    return new BasicBookFactory.Builder(content, bookMetaInfo).Build();

                case ContentType.SVG:
                    return new SvgBookFactory.Builder(new Uri(bookPath.ToString()), bookMetaInfo).Build();

                default:
                    throw new InvalidContentTypeException(
                        string.Format("Failed to create book as content type is not recognised for book " +
                                      "{0} at path {1}", bookId, bookPath));
            }
        }

        private UPath GetRootDir() {
            Debug.Log(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                applicationName, rootDirName));
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                applicationName, rootDirName);
        }

        private UPath GetLibraryManifestPath() {
            return Path.Combine(GetRootDir().ToString(), libraryManifestFilename);
        }

        private UPath GetBookLibDir() {
            return Path.Combine(GetRootDir().ToString(), booksLibDirName);
        }

        private UPath GetBookDir(string bookTitle) {
            return Path.Combine(GetBookLibDir().ToString(), bookTitle);
        }

        private UPath GetPagesDir(string bookTitle) {
            return Path.Combine(GetBookLibDir().ToString(), bookTitle, pagesDirName);
        }

        private UPath GetPagePath(string bookTitle, int pageNb, FileType fileType) {
            var extension = fileType.ToString().ToLower();
            return Path.Combine(GetPagesDir(bookTitle).ToString(), string.Format("{0}{1}.{2}",
                pagePrefix, pageNb, extension));
        }

        private UPath GetMetaInfoDir(string bookTitle) {
            return Path.Combine(GetBookDir(bookTitle).ToString(), metaDirName);
        }

        private UPath CreateBookDir(string bookTitle) {
            var bookDirPath = GetBookDir(bookTitle);
            physicalFileSystem.CreateDirectory(bookDirPath);
            return bookDirPath;
        }

        private UPath CreatePagesDir(string bookTitle) {
            var pagesDirPath = GetPagesDir(bookTitle);
            physicalFileSystem.CreateDirectory(pagesDirPath);
            return pagesDirPath;
        }

        private UPath CreateMetaInfoDir(string bookTitle) {
            var bookDirPath = GetMetaInfoDir(bookTitle);
            physicalFileSystem.CreateDirectory(bookDirPath);
            return bookDirPath;
        }

        private UPath SaveBookMetaInfo(BookMetaInfo bookMetaInfo, string originPath) {
            var metaInfoDir = CreateMetaInfoDir(bookMetaInfo.title);
            var bookMetaYaml = bookMetaInfo.Serialize();
            var fileContents = GenerateStreamFromString(bookMetaYaml);
            var filename = FileUtils.FileNameFromPath(
                               originPath, false) + bookMetaPostfix;
            UPath outputPath = Path.Combine(metaInfoDir.ToString(), filename);
            SaveFile(fileContents, outputPath);
            return outputPath;
        }

        private BookManifest CreateBookEntry(UPath vfsBookPath, Uri originalPath, UPath bookMetaInfoPath,
            BookMetaInfo bookMetaInfo, ContentType contentType) {
            var ext = FileUtils.FileExtFromPath(originalPath.AbsolutePath);
            FileType fileType;
            var success = Enum.TryParse(ext, true, out fileType);

            if (!success)
                throw new UnsupportFileFormatException(string.Format("Failed to create book entry from path " +
                                                                     "{0} as file format {1} is not supported",
                    originalPath, ext));

            var bookManifest = new BookManifest(GenerateId(), bookMetaInfo.title, vfsBookPath.ToString(),
                originalPath.AbsolutePath, bookMetaInfoPath.ToString(), contentType, fileType);
            return bookManifest;
        }

        private BookMetaInfo ReadBookMetaInfoFromVfs(string bookId) {
            UPath metaInfoPath = libraryManifest.GetBookById(bookId).metaInfoLocation;
            if (physicalFileSystem.FileExists(metaInfoPath)) {
                var metaInfoYaml = ReadFileAsStringFromVfs(metaInfoPath);
                var bookMetaInfo = BookMetaInfo.Deserialize(metaInfoYaml);
                return bookMetaInfo;
            }

            throw new BookNotFoundException("Unable to find book with id " + bookId);
        }

        private string ReadFileAsStringFromVfs(UPath uPath) {
            if (physicalFileSystem.FileExists(uPath)) return physicalFileSystem.ReadAllText(uPath);

            throw new FileNotFoundException("File " + uPath + " not found in virtual file system");
        }

        private string[] ReadFileAsLinesFromVfs(UPath uPath) {
            if (physicalFileSystem.FileExists(uPath)) return physicalFileSystem.ReadAllLines(uPath);

            throw new FileNotFoundException("File " + uPath + " not found in virtual file system");
        }

        private byte[] ReadFileAsBytesFromVfs(UPath uPath) {
            if (physicalFileSystem.FileExists(uPath)) return physicalFileSystem.ReadAllBytes(uPath);

            throw new FileNotFoundException("File " + uPath + " not found in virtual file system");
        }

        private string ReadFileAsStringFromNative(Uri path) {
            if (path.IsFile && File.Exists(path.AbsolutePath)) return File.ReadAllText(path.AbsolutePath);

            throw new FileNotFoundException("File " + path + " not found in native file system");
        }

        private bool BookExists(UPath uPath) {
            return physicalFileSystem.FileExists(uPath);
        }

        private void SaveFile(Stream fileInputStream, UPath destinationPath) {
            var outputStream = physicalFileSystem.CreateFile(destinationPath);
            fileInputStream.CopyTo(outputStream);
        }

        private UPath AsUpath(string path) {
            return new UPath(path);
        }

        public string GenerateId() {
            return Guid.NewGuid().ToString("D");
        }

        private static Stream GenerateStreamFromString(string s) {
            Stream stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}