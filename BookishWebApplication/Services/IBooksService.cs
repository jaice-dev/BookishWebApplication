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
            using var connection = new NpgsqlConnection(connectionString);

            var countPrintsQuery = "SELECT bookId, COUNT(id) FROM print GROUP BY bookId";

            var countsByBookId = connection.Query(countPrintsQuery).ToDictionary(row => (int) row.bookid, row => (int) row.count);

            var bookDictionary = new Dictionary<int, Book>();

            var getBooksQuery =
                "SELECT book.id as bookId, title, publicationyear, isbn, author.id as authorId, firstname, lastname FROM bookauthor, book, author WHERE bookauthor.bookid = book.id AND bookauthor.authorid = author.id";

            return connection.Query<Book, Author, Book>(
                    getBooksQuery,
                    (book, author) =>
                    {
                        Book currentBook;

                        if (!bookDictionary.TryGetValue(book.BookId, out currentBook))
                        {
                            currentBook = book;
                            currentBook.Authors = new List<Author>();
                            currentBook.PrintCount = countsByBookId.ContainsKey(currentBook.BookId) ? countsByBookId[currentBook.BookId] : 0;
                            bookDictionary.Add(currentBook.BookId, currentBook);
                        }
                        
                        currentBook.Authors.Add(author);

                        return currentBook;
                    },
                    splitOn: "authorId"
                )
                .Distinct();
        }
        

        public IEnumerable<Book> SearchBooks(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                var parameters = new DynamicParameters( new { SearchTitle = "%" + searchString + "%" } );
                var bookDictionary = new Dictionary<int, Book>();

                var sql = "SELECT book.id as bookId, title, publicationyear, isbn, author.id as authorId, firstname, lastname FROM bookauthor, book, author WHERE bookauthor.bookid = book.id AND bookauthor.authorid = author.id AND lower(title) LIKE lower(@SearchTitle)";

                using var connection = new NpgsqlConnection(connectionString);
                return connection.Query<Book, Author, Book>(
                        sql,
                        (book, author) =>
                        {
                            Book currentBook;

                            if (!bookDictionary.TryGetValue(book.BookId, out currentBook))
                            {
                                currentBook = book;
                                currentBook.Authors = new List<Author>();
                                bookDictionary.Add(currentBook.BookId, currentBook);
                            }
                        
                            currentBook.Authors.Add(author);

                            return currentBook;
                        },
                        splitOn: "authorId",
                        param: parameters
                    )
                    .Distinct();
            }

            return new List<Book>();
        }
    }
}