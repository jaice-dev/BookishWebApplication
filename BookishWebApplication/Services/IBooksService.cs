using System.Collections.Generic;
using BookishWebApplication.Models.Database;
using Dapper;
using Npgsql;

namespace BookishWebApplication.Services
{
    public interface IBooksService
    {
        IEnumerable<Book> GetAllBooks();
    }

    public class BooksService : IBooksService
    {
        // private const string connectionString = "Server=guineapig.zoo.lan;Port=5432;Database=bookishDB;Username=bookish;Password=softwire";
        private const string connectionString = "Server=localhost;Port=5432;Database=bookishDB;Username=bookish;Password=softwire";


        public IEnumerable<Book> GetAllBooks()
        {
            using var connection = new NpgsqlConnection(connectionString);
            return connection.Query<Book>("SELECT * FROM book");
        }
    }
}