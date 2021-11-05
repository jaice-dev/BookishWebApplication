using System.Collections.Generic;
using System.Linq;
using BookishWebApplication.Models.Database;
using BookishWebApplication.Models.Database.Create;
using BookishWebApplication.Models.Database.Delete;
using Dapper;
using Npgsql;

namespace BookishWebApplication.Services
{
    public interface IBooksService
    {
        IEnumerable<Book> GetAllBooks();
        IEnumerable<Book> GetBooksByAuthor(int authorId);
        IEnumerable<Book> SearchBooks(string query);
        Book GetBook(int id);
        int CreateBook(CreateBookModel newBook);
        void DeleteBook(DeleteBookModel book);
        void CreateBookCopy(CreateBookCopyModel newCopy);
        void DeleteBookCopy(CreateBookCopyModel newCopy);

        void AddAuthorToBook(BookAuthor bookAuthor);
        void DeleteAuthorFromBook(BookAuthor bookAuthor);
        void AddBookToAuthor(BookAuthor bookAuthor);
        void RemoveBookFromAuthor(BookAuthor bookAuthor);
    }

    public class BooksService : IBooksService
    {
        // private const string connectionString = "Server=guineapig.zoo.lan;Port=5432;Database=bookishDB;Username=bookish;Password=softwire";
        private const string ConnectionString = "Server=localhost;Port=5432;Database=bookishDB;Username=bookish;Password=softwire";

        public Book GetBook(int searchId)
        {
            var searchParameters = new DynamicParameters(new {SearchId = searchId});
            
            var getBooksQuery =
                @"SELECT book.id as bookId, title, publicationyear, isbn, authorid, firstname, lastname 
            from book LEFT OUTER JOIN bookauthor on book.id = bookauthor.bookid
            LEFT OUTER JOIN author on authorid = author.id where book.id = @SearchId;";

            return GetDatabaseBookResponse(getBooksQuery, searchParameters).First();
        }
        
        public IEnumerable<Book> GetBooksByAuthor(int authorId)
        {
            var searchParameters = new DynamicParameters(new {AuthorId = authorId});
            var getBooksQuery =
                @"SELECT book.id as bookId, title, publicationyear, isbn, authorid, firstname, lastname 
            from book LEFT OUTER JOIN bookauthor on book.id = bookauthor.bookid
            LEFT OUTER JOIN author on authorid = author.id WHERE bookauthor.authorid = @AuthorId;";
            
            return GetDatabaseBookResponse(getBooksQuery, searchParameters);
        }

        
        public IEnumerable<Book> GetAllBooks()
        {
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
                    @"SELECT book.id as bookId, title, publicationyear, isbn, authorid, firstname, lastname 
                    from book LEFT OUTER JOIN bookauthor on book.id = bookauthor.bookid
                    LEFT OUTER JOIN author on authorid = author.id WHERE (lower(title) LIKE lower(@SearchTitle));";
                
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

        public int CreateBook(CreateBookModel newBook)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                if (newBook.PublicationYear == 0)
                {
                    newBook.PublicationYear = null;
                }
                var sqlStatement =
                    @"INSERT INTO book (title, publicationyear, isbn) VALUES (@Title, @PublicationYear, @Isbn) RETURNING id";
                var result = connection.QuerySingle<int>(sqlStatement, newBook);
                return result;
            }
        }

        public async void DeleteBook(DeleteBookModel book)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                var sqlStatement =
                    @"DELETE FROM book where (id = @BookId)";
                await connection.OpenAsync();
                await connection.ExecuteAsync(sqlStatement, book);
            }
        }

        public async void CreateBookCopy(CreateBookCopyModel newBook)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                var sqlStatement =
                    "INSERT INTO print (bookid) VALUES (@BookId)";
                await connection.ExecuteAsync(sqlStatement, newBook);
            }
        }
        
        public async void DeleteBookCopy(CreateBookCopyModel newBook)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                var sqlStatement =
                    @"DELETE FROM print WHERE id IN (
                        SELECT id FROM
                        print WHERE bookid=@BookId LIMIT 1)";
                await connection.ExecuteAsync(sqlStatement, newBook);
            }
        }
        
        public async void AddAuthorToBook(BookAuthor bookAuthor)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                var sqlStatement =
                    @"INSERT INTO bookauthor (bookid, authorid) SELECT @BookId, @AuthorId WHERE NOT EXISTS (SELECT bookid, authorid FROM bookauthor WHERE (bookid = @BookId AND authorid = @AuthorId))";
                await connection.OpenAsync();
                await connection.ExecuteAsync(sqlStatement, bookAuthor);
            }
        }
        
        public async void DeleteAuthorFromBook(BookAuthor bookAuthor)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                var sqlStatement =
                    @"DELETE FROM bookauthor where (authorid = @AuthorId AND bookid = @BookId)";
                await connection.OpenAsync();
                await connection.ExecuteAsync(sqlStatement, bookAuthor);
            }
        }
        
        public async void AddBookToAuthor(BookAuthor bookAuthor)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                var sqlStatement =
                    @"INSERT INTO bookauthor (bookid, authorid) SELECT @BookId, @AuthorId WHERE NOT EXISTS (SELECT bookid, authorid FROM bookauthor WHERE (bookid = @BookId AND authorid = @AuthorId))";
                await connection.OpenAsync();
                await connection.ExecuteAsync(sqlStatement, bookAuthor);
            }
        }

        public async void RemoveBookFromAuthor(BookAuthor bookAuthor)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                var sqlStatement =
                    @"DELETE FROM bookauthor WHERE (bookid = @BookId AND authorid = @AuthorId)";
                await connection.OpenAsync();
                await connection.ExecuteAsync(sqlStatement, bookAuthor);
            }
        }
        
        
        
    }
}
