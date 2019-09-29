using System;
using System.Collections.Generic;
using System.IO;
using Modules.Book;
using Modules.Common;

namespace Modules.Library {

    public class Indexer {

        private VirtualFileLibrary virtualFileLibrary = new VirtualFileLibrary();

        public static void index(Uri inputPath, Uri outputDir, string bookTitle, ContentType contentType,
            params KeyValuePair<Option, object>[] options) {
            switch (contentType) {
                case ContentType.TEXT_ONLY:
                    var linesPerPage = resolveOption<int>(Option.LINES_PER_PAGE, options);
                    indexAsTxts(inputPath, outputDir, bookTitle, linesPerPage);
                    break;

                case ContentType.SVG:
                    indexAsSvgs(inputPath, outputDir, bookTitle);
                    break;
            }
        }

        private static void indexAsTxts(Uri inputPath, Uri outputDir, string bookTitle, int linesPerPage) {
            var content = File.ReadAllLines(inputPath.AbsolutePath);

            var pageNb = 1;
            for (var i = 0; i < content.Length; i += linesPerPage) {
                SubArray<string> pageContent;
                if (i + linesPerPage >= content.Length)
                    pageContent = new SubArray<string>(content, i, content.Length - i);
                else
                    pageContent = new SubArray<string>(content, i, linesPerPage);

                var filename = string.Format("Page{0}.txt", pageNb);
                var outputPath = Path.Combine(outputDir.AbsolutePath, filename);
                File.WriteAllLines(outputPath, pageContent.ToArray());
                pageNb++;
            }
        }

        private void generatePages(Uri inputPath, string outputDir) {
        }

        private TextPage generatePage(int pageNb, SubArray<string> pageContent) {
            var pageName = string.Format("Page {0}", pageNb);
            var lines = pageContent.ToArray();
            return new TextPage(pageName, pageNb, lines);
        }

        private static void indexAsSvgs(Uri inputPath, Uri outputDir, string bookTitle) {
            BookConverter.PdfToSvgs(inputPath, outputDir, bookTitle);
        }

        // todo handle casting issues
        private static T resolveOption<T>(Option customOption, params KeyValuePair<Option, object>[] pairs) {
            foreach (var option in pairs)
                if (option.Key == customOption)
                    return (T) option.Value;
            return (T) customOption.getOptionDefault();
        }
    }
}