using System;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using Modules.Book.Tests.Book;

namespace Modules.Library {
    
    public class SqlConn {

        private readonly string SQL_DB_NAME = "book-lib";
        private readonly string BOOKS_TABLE_NAME = "books";
        
        private readonly IDbConnection dbConn;
        
        public SqlConn() {
            string connection = "URI=file:" + Application.persistentDataPath + "/" + SQL_DB_NAME;
            dbConn = new SqliteConnection(connection);
            dbConn.Open();
            
            initBooksTable();
            initMetaInfoTable();
        }

        private void initBooksTable() {
            IDbCommand dbcmd;
            IDataReader reader;
            
            dbcmd = dbConn.CreateCommand();
            string query = 
                @"CREATE TABLE IF NOT EXISTS my_table (
                        id INTEGER PRIMARY KEY, 
                        book_id VARCHAR,
                        origin_url VARCHAR,
                        binding VARCHAR,
                        format VARCHAR)";
  
            dbcmd.CommandText = query;
            reader = dbcmd.ExecuteReader();
            reader.Close();
        }

        private void initMetaInfoTable() {
            IDbCommand dbcmd;
            IDataReader reader;
            
            dbcmd = dbConn.CreateCommand();
            string query = 
                @"CREATE TABLE IF NOT EXISTS my_table (
                        id INTEGER PRIMARY KEY AUTOINCREMENT, 
                        book_id VARCHAR,
                        title VARCHAR,
                        author VARCHAR,
                        publisher VARCHAR,
                        lang VARCHAR,
                        description VARCHAR,
                        category VARCHAR,
                        tags VARCHAR,
                        publication_date DATE,
                        page_count INTEGER,
                        FOREIGN KEY(book_id) REFERENCES " + BOOKS_TABLE_NAME + "(book_id))";
  
            dbcmd.CommandText = query;
            reader = dbcmd.ExecuteReader();
            reader.Close();
        }

        public void insertIntoBooks(BasicBook basicBook) {
            IDbCommand cmnd = dbConn.CreateCommand();
            cmnd.CommandText = String.Format("INSERT INTO my_table (" +
                          "book_id, origin_url, binding, format) " +
                          "VALUES ({}, {}, {}, {})", 
                basicBook.getBookId(),
                basicBook.getOriginUrl(),
                basicBook.getBinding(),
                basicBook.getBookFormat()
            );
            cmnd.ExecuteNonQuery();
        }
        
        public void insertIntoBooks(PdfBasicBook basicBook) {
            IDbCommand cmnd = dbConn.CreateCommand();
            cmnd.CommandText = String.Format("INSERT INTO my_table (" +
                                             "book_id, origin_url, binding, format) " +
                                             "VALUES ({}, {}, {}, {})", 
                basicBook.getBookId(),
                basicBook.getOriginUrl(),
                basicBook.getBinding(),
                basicBook.getBookFormat()
            );
            cmnd.ExecuteNonQuery();
        }
    }
}