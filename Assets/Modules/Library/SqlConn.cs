using System;
using UnityEngine;
using System.Data;
using Modules.Book;
using Modules.Book.Tests.Common;
using Mono.Data.Sqlite;

namespace Modules.Library {
    
    public class SqlConn {

        private readonly string SQL_DB_NAME = "book-lib";
        
        private readonly string BOOK_TABLE_NAME = "Book";
        private readonly string BOOK_META_TABLE_NAME = "BookMeta";
        private readonly string TEXT_BOOK_TABLE_NAME = "BasicBook";
        private readonly string SVG_BOOK_TABLE_NAME = "PdfSvgBook";

        private readonly string PAGE_TABLE_NAME = "Page";
        private readonly string TEXT_PAGE_TABLE_NAME = "TextPage";
        private readonly string SVG_PAGE_TABLE_NAME = "ImagePage";

        private readonly IDbConnection dbConn;
        
        public SqlConn() {
            string connection = "URI=file:" + Application.persistentDataPath + "/" + SQL_DB_NAME;
            dbConn = new SqliteConnection(connection);
            dbConn.Open();
            
            initBooksTable();
            initMetaInfoTable();
            initTextBookTable();
            initSvgBookTable();
            
            initPageTable();
            initTextPageTable();
            initSvgPageTable();
        }

        private void initBooksTable() {
            IDbCommand dbcmd;
            IDataReader reader;
            
            dbcmd = dbConn.CreateCommand();
            string query = String.Format("CREATE TABLE IF NOT EXISTS {0} (" +
                                         "id INTEGER PRIMARY KEY, " +
                                         "book_id VARCHAR NOT NULL UNIQUE, " +
                                         "origin_url VARCHAR, " +
                                         "binding VARCHAR, " +
                                         "format VARCHAR)", BOOK_TABLE_NAME);
  
            dbcmd.CommandText = query;
            reader = dbcmd.ExecuteReader();
            reader.Close();
        }

        private void initMetaInfoTable() {
            IDbCommand dbcmd;
            IDataReader reader;
            
            dbcmd = dbConn.CreateCommand();
            string query = String.Format("CREATE TABLE IF NOT EXISTS {0} ("+
                                         "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                         "book_id VARCHAR, " +
                                         "title VARCHAR, " +
                                         "author VARCHAR, " +
                                         "publisher VARCHAR, " +
                                         "language VARCHAR, " +
                                         "description VARCHAR, " +
                                         "category VARCHAR, " +
                                         "tags VARCHAR, " +
                                         "publication_date DATE, " +
                                         "page_count INTEGER, " +
                                         "FOREIGN KEY(book_id) REFERENCES {1} (book_id))", 
                BOOK_META_TABLE_NAME, BOOK_TABLE_NAME);
  
            dbcmd.CommandText = query;
            reader = dbcmd.ExecuteReader();
            reader.Close();
        }
        
        private void initTextBookTable() {
            IDbCommand dbcmd;
            IDataReader reader;
            
            dbcmd = dbConn.CreateCommand();
            string query = String.Format("CREATE TABLE IF NOT EXISTS {0} (" +
                                         "id INTEGER PRIMARY KEY AUTOINCREMENT, " + 
                                         "book_id VARCHAR, " +
                                         "lines_per_page INTEGER, " +
                                         "FOREIGN KEY(book_id) REFERENCES {1}(book_id))", 
                TEXT_BOOK_TABLE_NAME, BOOK_TABLE_NAME);
  
            dbcmd.CommandText = query;
            reader = dbcmd.ExecuteReader();
            reader.Close();
        }
        
        private void initSvgBookTable() {
            IDbCommand dbcmd;
            IDataReader reader;
            
            dbcmd = dbConn.CreateCommand();
            string query = String.Format("CREATE TABLE IF NOT EXISTS {0} (" +
                                         "id INTEGER PRIMARY KEY AUTOINCREMENT, " + 
                                         "book_id VARCHAR, " +
                                         "FOREIGN KEY(book_id) REFERENCES {1}(book_id))", 
                SVG_BOOK_TABLE_NAME, BOOK_TABLE_NAME);
  
            dbcmd.CommandText = query;
            reader = dbcmd.ExecuteReader();
            reader.Close();
        }

        private void initPageTable() {
            IDbCommand dbcmd;
            IDataReader reader;
            
            dbcmd = dbConn.CreateCommand();
            string query = String.Format("CREATE TABLE IF NOT EXISTS {0} (" +
                                         "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                         "book_id VARCHAR, " +
                                         "page_name VARCHAR, " +
                                         "page_number INTEGER, " +
                                         "content_type VARCHAR, " +
                                         "FOREIGN KEY(book_id) REFERENCES {1}(book_id))", 
                PAGE_TABLE_NAME, BOOK_TABLE_NAME);
  
            dbcmd.CommandText = query;
            reader = dbcmd.ExecuteReader();
            reader.Close();
        }

        private void initTextPageTable() {
            IDbCommand dbcmd;
            IDataReader reader;
            
            dbcmd = dbConn.CreateCommand();
            string query = String.Format("CREATE TABLE IF NOT EXISTS {0} (" +
                                         "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                         "page_id INTEGER, " +
                                         "lines TEXT, " +
                                         "FOREIGN KEY(page_id) REFERENCES {1}(id))", 
                TEXT_PAGE_TABLE_NAME, PAGE_TABLE_NAME);
  
            dbcmd.CommandText = query;
            reader = dbcmd.ExecuteReader();
            reader.Close();
        }
        
        private void initSvgPageTable() {
            IDbCommand dbcmd;
            IDataReader reader;
            
            dbcmd = dbConn.CreateCommand();
            string query = String.Format("CREATE TABLE IF NOT EXISTS {0} (" +
                                         "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                         "page_id INTEGER, " +
                                         "image TEXT, " +
                                         "FOREIGN KEY(page_id) REFERENCES {1}(id))", 
                SVG_PAGE_TABLE_NAME, PAGE_TABLE_NAME);
  
            dbcmd.CommandText = query;
            reader = dbcmd.ExecuteReader();
            reader.Close();
        }
        
        public void insertIntoBook(string bookId, string originUrl, Binding binding, BookFormat format, BookMetaInfo bookMetaInfo) {
            IDbCommand cmd = dbConn.CreateCommand();
            cmd.CommandText = String.Format("INSERT INTO {0} (" +
                                             "book_id, origin_url, binding, format) " +
                                             "VALUES ('{1}', '{2}', '{3}', '{4}')", 
                                              BOOK_TABLE_NAME, bookId, originUrl, binding, format);
            cmd.ExecuteNonQuery();
            insertIntoMeta(bookId, bookMetaInfo);
        }

        public void insertIntoBook(BasicBook basicBook) {
            insertIntoBook(basicBook.getBookId(), basicBook.getOriginUrl(), basicBook.getBinding(), 
                basicBook.getBookFormat(), basicBook.getBookMetaInfo());
            
            IDbCommand cmd = dbConn.CreateCommand();
            cmd.CommandText = String.Format("INSERT INTO {0} (" +
                                             "book_id, lines_per_page ) " +
                                             "VALUES ('{1}', '{2}')",
                TEXT_BOOK_TABLE_NAME, basicBook.getBookId(), basicBook.linesPerPage.ToString());
            cmd.ExecuteNonQuery();

            foreach (TextPage textPage in basicBook.pages) {
                insertIntoPage(basicBook.getBookId(), textPage);
            }
        }
        
        public void insertIntoBook(PdfSvgBook pdfSvgBook) {
            insertIntoBook(pdfSvgBook.getBookId(), pdfSvgBook.getOriginUrl(), pdfSvgBook.getBinding(), 
                pdfSvgBook.getBookFormat(), pdfSvgBook.getBookMetaInfo());
            
            IDbCommand cmd = dbConn.CreateCommand();
            cmd.CommandText = String.Format("INSERT INTO {0} (" +
                                            "book_id) " +
                                            "VALUES ('{1}')",
                TEXT_BOOK_TABLE_NAME, pdfSvgBook.getBookId());
            cmd.ExecuteNonQuery();
            
            foreach (SvgImagePage svgPage in pdfSvgBook.pages) {
                insertIntoPage(pdfSvgBook.getBookId(), svgPage);
            }
        }

        public int insertIntoPage(string bookId, string pageName, int pageNb, ContentType contentType) {
            IDbCommand cmd = dbConn.CreateCommand();
            cmd.CommandText = String.Format("INSERT INTO {0} (" +
                                            "book_id, page_name, page_number, content_type) " +
                                            "VALUES ('{1}', '{2}', '{3}', '{4}')",
                PAGE_TABLE_NAME, bookId, pageName, pageNb, contentType);
            
            return cmd.ExecuteNonQuery();
        }

        public void insertIntoPage(string bookId, TextPage textPage) {
            int pageId = insertIntoPage(bookId, textPage.getPageName(), textPage.getPageNb(), textPage.getContentType());
            
            IDbCommand cmd = dbConn.CreateCommand();
            
            
            cmd.CommandText = String.Format("INSERT INTO {0} (" +
                                            "page_id, lines) " +
                                            "VALUES ('{1}', '{2}')",
                TEXT_PAGE_TABLE_NAME, pageId, textPage.collapseLines().Replace("'","''"));
            
            cmd.ExecuteNonQuery();
        }
        
        public void insertIntoPage(string bookId, SvgImagePage svgImagePage) {
            int pageId = insertIntoPage(bookId, svgImagePage.getPageName(), svgImagePage.getPageNb(), 
                svgImagePage.getContentType());
            
            IDbCommand cmd = dbConn.CreateCommand();
            cmd.CommandText = String.Format("INSERT INTO {0} (" +
                                            "page_id, image) " +
                                            "VALUES ('{1}', '{2}')",
                TEXT_PAGE_TABLE_NAME, pageId, svgImagePage.image);
            cmd.ExecuteNonQuery();
        }

        public void insertIntoMeta(string bookId, BookMetaInfo bookMetaInfo) {
            IDbCommand cmd = dbConn.CreateCommand();
            cmd.CommandText = String.Format("INSERT INTO {0} (" +
                                            "book_id, " +
                                            "title, " +
                                            "author, " +
                                            "publisher, " +
                                            "language, " +
                                            "description, " +
                                            "category, " +
                                            "tags, " +
                                            "publication_date, " +
                                            "page_count) " +
                                            "VALUES (" +
                                            "'{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}')",
                                            BOOK_META_TABLE_NAME, 
                                            bookId, 
                                            bookMetaInfo.title, 
                                            bookMetaInfo.author, 
                                            bookMetaInfo.publisher, 
                                            bookMetaInfo.language,
                                            bookMetaInfo.description,
                                            bookMetaInfo.category,
                                            bookMetaInfo.collapseTags(),
                                            bookMetaInfo.publicationDate,
                                            bookMetaInfo.pageCount);
            cmd.ExecuteNonQuery();
        }
    }
}