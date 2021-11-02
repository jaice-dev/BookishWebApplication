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
            var test = "%" + searchString + "%";
            var parameters = new DynamicParameters( new { SearchTitle = test } );
            var sql = "SELECT * FROM book WHERE title LIKE @SearchTitle";
            using var connection = new NpgsqlConnection(connectionString);
            return connection.Query<Book>(sql, parameters);
        }
    }
}