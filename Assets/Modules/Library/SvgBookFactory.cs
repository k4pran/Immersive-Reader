using System;
using System.Collections.Generic;
using System.IO;
using Modules.Book;
using Modules.Common;
using File = UnityEngine.Windows.File;

namespace Modules.Library {
    
    public class SvgBookFactory {
        
        public class Builder : IBookFactory<BasicBook, TextPage> {

            private Uri bookPath;
            private FileType fileType = FileType.PDF;
            private BookMetaInfo bookMetaInfo;
            private Binding binding = Binding.DOUBLE_PAGED;

            public Builder(Uri bookPath, BookMetaInfo bookMetaInfo) {
                this.bookPath = bookPath;
                this.bookMetaInfo = bookMetaInfo;
            }

            public Builder setBinding(Binding binding) {
                this.binding = binding;
                return this;
            }

            public BasicBook build() {
                if (fileType == FileType.PDF) {
                    
                }

                throw new NotImplementedException();
            }

            public PdfSvgBook buildFromPdf() {
                Uri svgDir = convertPages();
                string[] svgs = getSvgsFromPath(svgDir);
                List<SvgPage> svgPages = new List<SvgPage>();
                return new PdfSvgBook(bookMetaInfo, binding, svgPages);
            }

            private string[] getSvgsFromPath(Uri svgDir) {
                string[] filePaths = Directory.GetFiles(svgDir.AbsolutePath);
                string[] svgs = new string[filePaths.Length];
                for (int i = 0; i < filePaths.Length; i++) {
                    if (File.Exists(filePaths[i]) && filePaths[i].EndsWith(".svg")) { // todo better checking for svg
                        svgs[i] = filePaths[i];
                    }
                }

                return svgs;
            }

            private Uri convertPages() {
                Uri tmpPath = new Uri(Path.Combine(Path.GetTempPath(), bookMetaInfo.title));
                BookConverter.pdfToSvgs(bookPath, tmpPath, bookMetaInfo.title);
                return tmpPath;
            }

            private List<SvgPage> generatePages(string[] svgs) {
                List<SvgPage> pages = new List<SvgPage>();
                for (int pageNumber = 0; pageNumber < svgs.Length; pageNumber++) {
                    string pageName = String.Format("Page {0}", pageNumber);
                    pages.Add(new SvgPage(pageName, pageNumber, svgs[pageNumber]));
                }

                return pages;
            }
        }
        
    }
}