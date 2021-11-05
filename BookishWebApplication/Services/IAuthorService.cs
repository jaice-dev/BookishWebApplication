using System.Collections.Generic;
using System.Linq;
using BookishWebApplication.Models.Database;
using BookishWebApplication.Models.Database.Create;
using Dapper;
using Npgsql;

namespace BookishWebApplication.Services
{

    public interface IAuthorService
    {
        // IEnumerable<Author> GetAuthor(int id);
        IEnumerable<Author> GetAllAuthors();
        Author GetAuthor(int id);
        int CreateAuthor(CreateAuthorModel newAuthor);
        void DeleteAuthor(DeleteAuthorModel author);

    }

    public class AuthorService : IAuthorService
    {

        // private const string connectionString = "Server=guineapig.zoo.lan;Port=5432;Database=bookishDB;Username=bookish;Password=softwire";
        private const string ConnectionString =
            "Server=localhost;Port=5432;Database=bookishDB;Username=bookish;Password=softwire";

        public IEnumerable<Author> GetAllAuthors()
        {
            var getAuthorsQuery =
                "SELECT firstname, lastname, id as authorId from author";

            using var connection = new NpgsqlConnection(ConnectionString);

            return connection.Query<Author>(getAuthorsQuery);
        }
        
        public Author GetAuthor(int id)
        {
            var searchParameters = new DynamicParameters(new {SearchId = id});
            
            var getAuthorsQuery =
                "SELECT firstname, lastname, id as authorId from author where id = @SearchId";

            using var connection = new NpgsqlConnection(ConnectionString);

            return connection.Query<Author>(getAuthorsQuery, searchParameters).First();
        }
        
        public int CreateAuthor(CreateAuthorModel newAuthor)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                var sqlStatement =
                    @"INSERT INTO author (firstname, lastname) VALUES (@FirstName, @LastName) RETURNING id";
                var result = connection.QuerySingle<int>(sqlStatement, newAuthor);
                return result;
            }
        }
        
        public async void DeleteAuthor(DeleteAuthorModel author)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                var sqlStatement =
                    @"DELETE FROM author where id = @AuthorId";
                await connection.OpenAsync();
                await connection.ExecuteAsync(sqlStatement, author);
            }
        }
        
        
    }
}