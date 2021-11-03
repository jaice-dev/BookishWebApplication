using System.Collections.Generic;
using System.Linq;
using BookishWebApplication.Models.Database;
using Dapper;
using Npgsql;

namespace BookishWebApplication.Services
{
    public interface IBooksService
    {
        IEnumerable<Book> GetAllBooks();
        IEnumerable<Book> SearchBooks(string query);
    }

    public class BooksService : IBooksService
    {
        // private const string connectionString = "Server=guineapig.zoo.lan;Port=5432;Database=bookishDB;Username=bookish;Password=softwire";
        private const string connectionString = "Server=localhost;Port=5432;Database=bookishDB;Username=bookish;Password=softwire";


        public IEnumerable<Book> GetAllBooks()
        {
            var bookDictionary = new Dictionary<int, Book>();

            var sql =
                "SELECT * from bookauthor JOIN book on bookauthor.bookid = book.Id JOIN author on bookauthor.authorid = author.id";

            using var connection = new NpgsqlConnection(connectionString);
            return connection.Query<BookAuthor, Book, Author, Book>(
                    sql,
                    (bookAuthor, book, author) =>
                    {
                        Book currentBook;

                        if (!bookDictionary.TryGetValue(book.Id, out currentBook))
                        {
                            currentBook = book;
                            currentBook.Authors = new List<Author>();
                            bookDictionary.Add(currentBook.Id, currentBook);
                        }
                        
                        currentBook.Authors.Add(author);

                        return currentBook;
                    },
                    splitOn: "authorid"
                )
                .Distinct();
        }
        

        public IEnumerable<Book> SearchBooks(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                var parameters = new DynamicParameters( new { SearchTitle = "%" + searchString + "%" } );
                var sql = @"SELECT * FROM bookauthor
                            JOIN book on bookauthor.bookid = book.Id
                            JOIN author on bookauthor.authorid = author.id
                            WHERE lower(title) LIKE lower(@SearchTitle)";
                using var connection = new NpgsqlConnection(connectionString);
                return connection.Query<Book>(sql, parameters);
            }

            return new List<Book>();
        }
    }
}