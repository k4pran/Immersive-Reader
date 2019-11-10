using System;
using System.Collections.Generic;
using System.IO;
using Modules.Book;
using Modules.Common;
using File = UnityEngine.Windows.File;

namespace Modules.Library {

    public class SvgBookFactory {

        public class Builder : IBookFactory<SvgBook, SvgPage> {

            private Binding binding = Binding.DOUBLE_PAGED;
            private readonly BookMetaInfo bookMetaInfo;

            private readonly Uri bookPath;
            private readonly FileType fileType = FileType.PDF;

            public Builder(Uri bookPath, BookMetaInfo bookMetaInfo) {
                this.bookPath = bookPath;
                this.bookMetaInfo = bookMetaInfo;
            }

            public SvgBook Build() {
                Logger.Debug($"Building svg book {bookMetaInfo.title}");
                if (fileType == FileType.PDF) BuildFromPdf();

                Logger.Debug("Finished building svg book");
                throw new NotImplementedException();
            }

            public Builder SetBinding(Binding binding) {
                this.binding = binding;
                return this;
            }

            public SvgBook BuildFromPdf() {
                var svgDir = ConvertPages();
                var svgs = GetSvgsFromPath(svgDir);
                var svgPages = new List<SvgPage>();
                return new SvgBook(bookMetaInfo, binding, svgPages);
            }

            private string[] GetSvgsFromPath(Uri svgDir) {
                var filePaths = Directory.GetFiles(svgDir.AbsolutePath);
                var svgs = new string[filePaths.Length];
                for (var i = 0; i < filePaths.Length; i++)
                    if (File.Exists(filePaths[i]) && filePaths[i].EndsWith(".svg")) // todo better checking for svg
                        svgs[i] = filePaths[i];

                return svgs;
            }

            private Uri ConvertPages() {
                var tmpPath = new Uri(Path.Combine(Path.GetTempPath(), bookMetaInfo.title));
                BookConverter.PdfToSvgs(bookPath, tmpPath, bookMetaInfo.title);
                return tmpPath;
            }

            private List<SvgPage> GeneratePages(string[] svgs) {
                Logger.Debug($"Generating pages for svg book {bookMetaInfo.title}");
                var pages = new List<SvgPage>();
                int pageNumber = 1;
                for (int i = 0; i < svgs.Length; i++) {
                    Logger.Trace($"Generating page {pageNumber} for book {bookMetaInfo.title}");
                    string pageName = string.Format($"Page {pageNumber}");
                    pages.Add(new SvgPage(pageName, pageNumber, svgs[i]));
                    Logger.Trace("Page generated");
                    pageNumber++;
                }

                return pages;
            }
        }
    }
}