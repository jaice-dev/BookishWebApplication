using System.Collections.Generic;
using System.Linq;
using BookishWebApplication.Models.Database;
using BookishWebApplication.Models.Database.Create;
using Dapper;
using Npgsql;

namespace BookishWebApplication.Services
{
    public interface IBooksService
    {
        IEnumerable<Book> GetAllBooks();
        IEnumerable<Book> SearchBooks(string query);
        Book GetBook(int id);
        void CreateBook(CreateBookModel newBook);
        void CreateAuthor(CreateAuthorModel newAuthor);
        void CreateBookCopy(CreateBookCopyModel newCopy);
        void AddAuthorToBook(CreateBookCopyModel newCopy);

    }

    public class BooksService : IBooksService
    {
        // private const string connectionString = "Server=guineapig.zoo.lan;Port=5432;Database=bookishDB;Username=bookish;Password=softwire";
        private const string ConnectionString = "Server=localhost;Port=5432;Database=bookishDB;Username=bookish;Password=softwire";

        public Book GetBook(int searchId)
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            var getBookSql = "SELECT * from book WHERE (id = @searchId);";
            return new Book();
        }
        
        public IEnumerable<Book> GetAllBooks()
        {
            //Get the book
            //Search in bookauthor
            // if id is in bookauthor
            // get fistname and last name

            var getBooksQuery =
                @"SELECT book.id as bookId, title, publicationyear, isbn, authorid, firstname, lastname 
            from book LEFT OUTER JOIN bookauthor on book.id = bookauthor.bookid
            LEFT OUTER JOIN author on authorid = author.id;";
            
            return GetDatabaseBookResponse(getBooksQuery, null);
        }
        
        public IEnumerable<Book> SearchBooks(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                var searchParameters = new DynamicParameters(new {SearchTitle = "%" + searchString + "%"});

                var searchBooksQuery =
                    "SELECT book.id as bookId, title, publicationyear, isbn, author.id as authorId, firstname, lastname FROM bookauthor, book, author WHERE bookauthor.bookid = book.id AND bookauthor.authorid = author.id AND lower(title) LIKE lower(@SearchTitle)";

                return GetDatabaseBookResponse(searchBooksQuery, searchParameters);
            }

            return null;
        }

        private static IEnumerable<Book> GetDatabaseBookResponse(string searchSql, DynamicParameters parameters = null)
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            
            var countsByBookId = GetPrintCountDict(connection);

            var bookDictionary = new Dictionary<int, Book>();

            return connection.Query<Book, Author, Book>(
                    searchSql,
                    (book, author) =>
                    {
                        Book currentBook;

                        if (!bookDictionary.TryGetValue(book.BookId, out currentBook))
                        {
                            currentBook = book;
                            currentBook.Authors = new List<Author>();
                            currentBook.PrintCount = countsByBookId.ContainsKey(currentBook.BookId)
                                ? countsByBookId[currentBook.BookId]
                                : 0;
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

        private static Dictionary<int, int> GetPrintCountDict(NpgsqlConnection connection)
        {
            var countPrintsQuery = "SELECT bookId, COUNT(id) FROM print GROUP BY bookId";

            var countsByBookId =
                connection.Query(countPrintsQuery).ToDictionary(row => (int) row.bookid, row => (int) row.count);
            return countsByBookId;
        }

        public async void CreateBook(CreateBookModel newBook)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                if (newBook.PublicationYear == 0)
                {
                    newBook.PublicationYear = null;
                }
                await connection.OpenAsync();
                var sqlStatement =
                    @"INSERT INTO book (title, publicationyear, isbn) VALUES (@Title, @PublicationYear, @Isbn);";
                await connection.ExecuteAsync(sqlStatement, newBook);
            }
        }
        
        public async void CreateAuthor(CreateAuthorModel newAuthor)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                var sqlStatement =
                    @"INSERT INTO author (firstname, lastname) VALUES (@FirstName, @LastName);";
                await connection.ExecuteAsync(sqlStatement, newAuthor);
            }
        }
        
        public async void CreateBookCopy(CreateBookCopyModel newBook)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                var sqlStatement =
                    "wdw";
                await connection.ExecuteAsync(sqlStatement, newBook);
            }
        }
        
        public async void AddAuthorToBook(CreateBookCopyModel newBook)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                var sqlStatement =
                    @"WITH book_key AS (SELECT id from book where title = @Title),
 author_key as (SELECT id from author WHERE (firstname = @FirstName AND lastname = @LastName)) 
INSERT INTO bookauthor (bookid, authorid) SELECT book_key.id, author_key.id FROM book_key, author_key";
                await connection.ExecuteAsync(sqlStatement, newBook);
            }
        }
        
    }
}