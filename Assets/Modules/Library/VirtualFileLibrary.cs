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
using Logger = Modules.Common.Logger;

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
                    observer.OnNext(ReadBookMetaInfoFromVfs(bookId));
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
                    string filename = FileUtils.FileNameFromPath(bookInputPath.AbsolutePath);
                    UPath bookDirPath = CreateBookDir(bookMetaInfo.title);
                    Uri bookOutputPath = new Uri(Path.Combine(bookDirPath.ToString(), filename));
                    SaveBook(bookInputPath, bookOutputPath);

                    // Save meta info
                    UPath metaInfoLocation = SaveBookMetaInfo(bookMetaInfo, bookInputPath.AbsolutePath);

                    // Create book entry and add to library
                    BookManifest bookManifest = CreateBookEntry(bookOutputPath.AbsolutePath,
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
                    byte[] contents = ReadFileAsBytesFromVfs(bookUPath);
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
                    string contents = ReadFileAsStringFromVfs(bookUPath);
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
                    string[] contents = ReadFileAsLinesFromVfs(bookUPath);
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
                    string bookTitle = libraryManifest.GetBookById(bookId).bookTitle;
                    FileType fileType = libraryManifest.GetBookById(bookId).fileType;
                    UPath pagePath = GetPagePath(bookTitle, pageNb, fileType);

                    string contents = ReadFileAsStringFromVfs(pagePath);
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
                    string bookTitle = libraryManifest.GetBookById(bookId).bookTitle;
                    FileType fileType = libraryManifest.GetBookById(bookId).fileType;
                    UPath pagePath = GetPagePath(bookTitle, pageNb, fileType);

                    string[] contents = ReadFileAsLinesFromVfs(pagePath);
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
                    string bookTitle = libraryManifest.GetBookById(bookId).bookTitle;
                    FileType fileType = libraryManifest.GetBookById(bookId).fileType;
                    UPath pagePath = GetPagePath(bookTitle, pageNb, fileType);

                    byte[] contents = ReadFileAsBytesFromVfs(pagePath);
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
            Logger.Info($"Setting up physical file system at root {GetRootDir()}");
            physicalFileSystem.CreateDirectory(GetRootDir());
            physicalFileSystem.CreateDirectory(GetBookLibDir());
            InitializeLibrary();
        }

        private void InitializeLibrary() {
            Logger.Info($"Initializing library at {GetLibraryManifestPath()}");
            if (physicalFileSystem.FileExists(GetLibraryManifestPath())) {
                Logger.Debug($"Library already exists at {GetLibraryManifestPath()}");
                LoadExistingLibrary();
            }
            else {
                Logger.Debug("No library found. Creating now");
                CreateNewLibrary();
            }

            Logger.Info("Library ready");
        }

        private void LoadExistingLibrary() {
            Logger.Debug($"Loading library from {GetLibraryManifestPath()}");
            string libraryYamlContents = ReadFileAsStringFromVfs(GetLibraryManifestPath());
            libraryManifest = LibraryManifest.Deserialize(libraryYamlContents);
        }

        private void CreateNewLibrary() {
            libraryManifest = new LibraryManifest();
            string emptyLibraryYaml = libraryManifest.Serialize();
            SaveFile(GenerateStreamFromString(emptyLibraryYaml), GetLibraryManifestPath());
        }

        private void AddBookToLibraryManifest(BookManifest bookManifest) {
            libraryManifest.AddEntry(bookManifest);
            Logger.Debug($"Added book manifest [{bookManifest.bookTitle} : {bookManifest.bookId}] added to library");
        }

        private void SaveLibraryManifest() {
            string libraryYaml = libraryManifest.Serialize();
            Stream libraryStream = GenerateStreamFromString(libraryYaml);
            SaveFile(libraryStream, GetLibraryManifestPath());
        }

        private void IndexBook(string bookId, params KeyValuePair<Option, object>[] options) {
            BookManifest bookManifest = libraryManifest.GetBookById(bookId);
            Uri bookLocation = new Uri(bookManifest.bookLocation);
            Uri pagesDir = new Uri(CreatePagesDir(bookManifest.bookTitle).ToString());
            string bookTitle = bookManifest.bookTitle;
            ContentType contentType = bookManifest.contentType;

            Logger.Debug($"Indexing book [{bookTitle} : {bookId}] at {pagesDir}");
            Indexer.index(bookLocation, pagesDir, bookTitle, contentType, options);
        }

        private void SaveBook(Uri inputPath, Uri outputPath) {
            string fileContents = ReadFileAsStringFromNative(inputPath);

            Stream stream = GenerateStreamFromString(fileContents);
            UPath bookOutputPath = Path.Combine(outputPath.AbsolutePath);
            Logger.Debug($"Saving book from {inputPath} to target path {outputPath}");
            SaveFile(stream, bookOutputPath);
        }

        private IBook<Page> CreateBookWithFactory(string bookId) {
            ContentType contentType = libraryManifest.GetBookById(bookId).contentType;
            UPath bookPath = libraryManifest.GetBookById(bookId).bookLocation;
            BookMetaInfo bookMetaInfo = ReadBookMetaInfoFromVfs(bookId);

            switch (contentType) {
                case ContentType.TEXT_ONLY:
                    string[] content = ReadFileAsLinesFromVfs(bookPath);
                    Logger.Debug($"Creating text only book [{bookMetaInfo.title} : {bookId}] at {bookPath}");
                    return new BasicBookFactory.Builder(content, bookMetaInfo).Build();

                case ContentType.SVG:
                    Logger.Debug($"Creating svg book [{bookMetaInfo.title} : {bookId}] at {bookPath}");
                    return new SvgBookFactory.Builder(new Uri(bookPath.ToString()), bookMetaInfo).Build();

                default:
                    throw new InvalidContentTypeException(
                        "Failed to create book as content type is not recognised for book " +
                                      $"[{bookMetaInfo.title} : {bookId}] at path {bookPath}");
            }
        }

        private UPath GetRootDir() {
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
            string extension = fileType.ToString().ToLower();
            return Path.Combine(GetPagesDir(bookTitle).ToString(), string.Format("{0}{1}.{2}",
                pagePrefix, pageNb, extension));
        }

        private UPath GetMetaInfoDir(string bookTitle) {
            return Path.Combine(GetBookDir(bookTitle).ToString(), metaDirName);
        }

        private UPath CreateBookDir(string bookTitle) {
            UPath bookDirPath = GetBookDir(bookTitle);
            Logger.Debug($"Creating book directory at {bookDirPath} for book {bookTitle}");
            physicalFileSystem.CreateDirectory(bookDirPath);
            return bookDirPath;
        }

        private UPath CreatePagesDir(string bookTitle) {
            UPath pagesDirPath = GetPagesDir(bookTitle);
            Logger.Debug($"Creating pages directory at {pagesDirPath} for book {bookTitle}");
            physicalFileSystem.CreateDirectory(pagesDirPath);
            return pagesDirPath;
        }

        private UPath CreateMetaInfoDir(string bookTitle) {
            UPath metaInfoDir = GetMetaInfoDir(bookTitle);
            Logger.Debug($"Creating meta info at {metaInfoDir} for book {bookTitle}");
            physicalFileSystem.CreateDirectory(metaInfoDir);
            return metaInfoDir;
        }

        private UPath SaveBookMetaInfo(BookMetaInfo bookMetaInfo, string originPath) {
            UPath metaInfoDir = CreateMetaInfoDir(bookMetaInfo.title);
            string bookMetaYaml = bookMetaInfo.Serialize();
            Stream fileContents = GenerateStreamFromString(bookMetaYaml);
            string filename = FileUtils.FileNameFromPath(
                               originPath, false) + bookMetaPostfix;
            UPath outputPath = Path.Combine(metaInfoDir.ToString(), filename);
            Logger.Debug($"Saving book meta info {bookMetaInfo.title} at {outputPath}");
            SaveFile(fileContents, outputPath);
            return outputPath;
        }

        private BookManifest CreateBookEntry(UPath vfsBookPath, Uri originalPath, UPath bookMetaInfoPath,
            BookMetaInfo bookMetaInfo, ContentType contentType) {
            Logger.Debug($"Creating book entry from {originalPath} at {vfsBookPath}");
            string ext = FileUtils.FileExtFromPath(originalPath.AbsolutePath, withDot: false);
            FileType fileType;
            bool success = Enum.TryParse(ext, true, out fileType);

            if (!success) {
                throw new UnsupportFileFormatException(
                    $"Failed to create book entry from path {originalPath} as file format {ext} is not supported");
            }

            BookManifest bookManifest = new BookManifest(GenerateId(), bookMetaInfo.title, vfsBookPath.ToString(),
                originalPath.AbsolutePath, bookMetaInfoPath.ToString(), contentType, fileType);
            Logger.Debug($"Book entry created at {bookManifest.bookLocation} [{bookManifest.bookTitle} : {bookManifest.bookId}]");
            return bookManifest;
        }

        private BookMetaInfo ReadBookMetaInfoFromVfs(string bookId) {
            UPath metaInfoPath = libraryManifest.GetBookById(bookId).metaInfoLocation;
            Logger.Debug($"Reading book {bookId} at {metaInfoPath}");
            if (physicalFileSystem.FileExists(metaInfoPath)) {
                string metaInfoYaml = ReadFileAsStringFromVfs(metaInfoPath);
                BookMetaInfo bookMetaInfo = BookMetaInfo.Deserialize(metaInfoYaml);
                Logger.Debug($"Meta info found for book {bookId} at {metaInfoPath}");
                return bookMetaInfo;
            }

            Logger.Debug($"Book meta info for book {bookId} not found at {metaInfoPath}");
            throw new BookNotFoundException("Unable to find book with id " + bookId);
        }

        private string ReadFileAsStringFromVfs(UPath uPath) {
            Logger.Debug($"Reading file as string at {uPath}");
            if (physicalFileSystem.FileExists(uPath)) 
                return physicalFileSystem.ReadAllText(uPath);
            throw new FileNotFoundException($"File {uPath} not found in virtual file system");
        }

        private string[] ReadFileAsLinesFromVfs(UPath uPath) {
            Logger.Debug($"Reading file as lines at {uPath}");
            if (physicalFileSystem.FileExists(uPath)) 
                return physicalFileSystem.ReadAllLines(uPath);
            throw new FileNotFoundException($"File {uPath} not found in virtual file system");
        }

        private byte[] ReadFileAsBytesFromVfs(UPath uPath) {
            Logger.Debug($"Reading file as bytes at {uPath}");
            if (physicalFileSystem.FileExists(uPath)) 
                return physicalFileSystem.ReadAllBytes(uPath);
            throw new FileNotFoundException($"File {uPath} not found in virtual file system");
        }

        private string ReadFileAsStringFromNative(Uri path) {
            Logger.Debug($"Reading file as string at {path}");
            if (path.IsFile && File.Exists(path.AbsolutePath)) 
                return File.ReadAllText(path.AbsolutePath);
            throw new FileNotFoundException($"File {path} not found in native file system");
        }

        private bool BookExists(UPath uPath) {
            Logger.Trace($"Checking book exists at {uPath}");
            return physicalFileSystem.FileExists(uPath);
        }

        private void SaveFile(Stream fileInputStream, UPath destinationPath) {
            Logger.Debug($"Saving file input stream to {destinationPath}");
            Stream outputStream = physicalFileSystem.CreateFile(destinationPath);
            fileInputStream.CopyTo(outputStream);
            fileInputStream.Close();
            outputStream.Close();
        }

        public string GenerateId() {
            Logger.Trace("Generating new book id");
            return Guid.NewGuid().ToString("D");
        }

        private static Stream GenerateStreamFromString(string s) {
            Stream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}