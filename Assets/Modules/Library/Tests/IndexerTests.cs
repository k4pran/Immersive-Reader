using System;
using NUnit.Framework;

namespace Modules.Library.Tests {
    
    public class IndexerTests {

        [Test]
        public void testIndexingPdfAsSvgs() {
            Uri inputPath = new Uri(
                "/Users/ryan/Documents/Unity/VReader_2/Assets/Modules/Book/Tests/Resources/atari.pdf");
            Uri outputPath = new Uri(
                "/Users/ryan/Documents/Unity/VReader_2/Assets/dump");
            Indexer.asSvgs(inputPath, outputPath, "atari");
        }
    }
}