using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DefaultNamespace;
using Modules.Book;
using UnityEngine;
using Zio;
using Zio.FileSystems;

namespace Modules.Library {
    
    public class VirtualFileLibrary : ILibrary {

        private MemoryFileSystem memoryFileSystem;
        private readonly string ROOT_DIR = "/VReader Library";
        private readonly string LIBRARY_MANIFEST_FILENAME = "library";
        private readonly string BOOK_DIR = "books";
        private readonly UPath LIBRARY_MANIFEST_PATH;

        private LibraryManifest libraryManifest;
            
        public VirtualFileLibrary() {
            memoryFileSystem = new MemoryFileSystem();
            LIBRARY_MANIFEST_PATH = new UPath(Path.Combine(ROOT_DIR, LIBRARY_MANIFEST_FILENAME));
            
            setup();
        }

        public void setup() {
            memoryFileSystem.CreateDirectory(ROOT_DIR);
            memoryFileSystem.CreateDirectory(Path.Combine(ROOT_DIR, BOOK_DIR));
        }

        private void initializeLibrary() {
            Debug.Log("Initializing library...");
            if (memoryFileSystem.FileExists(LIBRARY_MANIFEST_PATH)) {
                Debug.Log("Library exists at " + LIBRARY_MANIFEST_PATH);
                loadExistingLibrary();
            }
            else {
                Debug.Log("No library found. Creating now...");
                createNewLibrary();
            }
            Debug.Log("Library ready");
        }

        private void loadExistingLibrary() {
            string libraryYamlContents = readFileAsString(LIBRARY_MANIFEST_PATH);
            libraryManifest = LibraryManifest.deserialize(libraryYamlContents);
        }

        private void createNewLibrary() {
            libraryManifest = new LibraryManifest();
            string emptyLibraryYaml = libraryManifest.serialize();
            saveFile(generateStreamFromString(emptyLibraryYaml), LIBRARY_MANIFEST_PATH);
        }

        public IObservable<LibraryManifest> retrieveLibraryManifest() {
            return Observable.Create<LibraryManifest>(observer => {
                observer.OnNext(libraryManifest);
                observer.OnCompleted();
                return Disposable.Empty;
            });
        }

        public IObservable<LibraryManifest> addBookToLibraryManifest(BookManifest bookManifest) {
            return Observable.Create<LibraryManifest>(observer => {
                try {
                    libraryManifest.addEntry(bookManifest);
                    observer.OnNext(libraryManifest);
                    saveLibraryManifest();
                    observer.OnCompleted();
                }
                catch (InvalidBookIDException e) {
                    observer.OnError(e);
                }
                return Disposable.Empty;
            });
        }

        public IObservable<LibraryManifest> saveLibraryManifest() {
            return Observable.Create<LibraryManifest>(observer => {
                memoryFileSystem.CreateFile(LIBRARY_MANIFEST_PATH);
                string updatedLibraryYaml = libraryManifest.serialize();
                
                saveFile(generateStreamFromString(updatedLibraryYaml), LIBRARY_MANIFEST_PATH);
                observer.OnNext(libraryManifest);
                observer.OnCompleted();
                return Disposable.Empty;
            });
        }

        public IObservable<BookManifest> importBook(Uri bookPath, BookMetaInfo bookMetaInfo = null) {
            throw new NotImplementedException();
        }

        public IObservable<BookManifest> retrieveBookManifest(string bookID) {
            throw new NotImplementedException();
        }

        public IObservable<Dictionary<string, string>> getLibraryManifest() {
            throw new NotImplementedException();
        }

        public IObservable<byte[]> retrieveBook(string bookId) {
            throw new NotImplementedException();
        }

        public IObservable<byte[]> retrieveBookMetaData(string bookId) {
            throw new NotImplementedException();
        }

        public IObservable<int> getBookCount() {
            throw new NotImplementedException();
        }

        public IObservable<int> getPageCount(string bookID) {
            throw new NotImplementedException();
        }

        private string readFileAsString(UPath uPath) {
            return memoryFileSystem.ReadAllText(uPath);
        }

        private void saveFile(Stream fileInputStream, UPath destinationPath) {
            Stream outputStream = memoryFileSystem.CreateFile(destinationPath);
            fileInputStream.CopyTo(outputStream);
        }

        private UPath asUpath(string path) {
            return new UPath(path);
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