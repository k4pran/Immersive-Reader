using System;
using NUnit.Framework;
using Common;

namespace EReader.Tests {

    public class BookLoaderTests {

        [Test]
        public void LoadingTextFilesReadAsLines() {
            BookLoader<string[]> bookLoader = new BookLoader<string[]>();
            string[] contents = bookLoader.loadText("Assets/Modules/EReader/Tests/Resources/dracula.txt");
            Assert.Greater(contents.Length, 0);
        }
        
        [Test]
        public void FileExtensionsExtractedCorrectly() {
            string ext = FileUtils.getFileExt("Assets/Modules/EReader/Tests/Resources/dracula.txt");
            Assert.AreEqual(ext, ".txt");
        }

        [Test]
        public void LoadingPdfFilesAsJpegs() {
            PdfConversion.toJpegs("Assets/Modules/EReader/Tests/Resources/atari.pdf", "Assets/Modules/EReader/Tests/Output/dracula");
        }
        
        [Test]
        public void LoadingPdfFilesAsSvgs() {
            PdfConversion.toSvgs("Assets/Modules/EReader/Tests/Resources/atari.pdf", "Assets/Modules/EReader/Tests/Output/atari");
        }
    }

}