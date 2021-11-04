using System.Collections.Generic;
using BookishWebApplication.Models.Database;
using Dapper;
using Npgsql;

namespace BookishWebApplication.Services
{

    public interface IAuthorService
    {
        // IEnumerable<Author> GetAuthor(int id);
        IEnumerable<Author> GetAllAuthors();
    }

    public class AuthorService : IAuthorService
    {

        // private const string connectionString = "Server=guineapig.zoo.lan;Port=5432;Database=bookishDB;Username=bookish;Password=softwire";
        private const string ConnectionString =
            "Server=localhost;Port=5432;Database=bookishDB;Username=bookish;Password=softwire";
        // public IEnumerable<Author> GetAuthor(int id)
        // {
        //     
        // }

        public IEnumerable<Author> GetAllAuthors()
        {
            var getAuthorsQuery =
                "SELECT firstname, lastname, id as authorId from author";

            using var connection = new NpgsqlConnection(ConnectionString);

            return connection.Query<Author>(getAuthorsQuery);
        }
    }
}