using System.Collections.Generic;
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
            return connection.Query<Book>("SELECT * FROM book ORDER BY title");
        }
        

        public IEnumerable<Book> SearchBooks(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                var parameters = new DynamicParameters( new { SearchTitle = "%" + searchString + "%" } );
                var sql = "SELECT * FROM book WHERE lower(title) LIKE lower(@SearchTitle)";
                using var connection = new NpgsqlConnection(connectionString);
                return connection.Query<Book>(sql, parameters);
            }

            return new List<Book>();
        }
    }
}