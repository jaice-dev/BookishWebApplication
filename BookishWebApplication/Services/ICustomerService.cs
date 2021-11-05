using System.Collections.Generic;
using System.Linq;
using BookishWebApplication.Models.Database;
using BookishWebApplication.Models.Database.Create;
using Dapper;
using Npgsql;

namespace BookishWebApplication.Services
{

    public interface ICustomerService
    {
        // IEnumerable<Author> GetAuthor(int id);
        IEnumerable<Customer> GetAllCustomers();
        Customer GetCustomer(int id);
        int CreateUser(CreateCustomerModel newCustomer);
        void DeleteUser(DeleteCustomerModel user);
        void EditUserDetails(Customer user);

    }

    public class CustomerService : ICustomerService
    {

        // private const string connectionString = "Server=guineapig.zoo.lan;Port=5432;Database=bookishDB;Username=bookish;Password=softwire";
        private const string ConnectionString =
            "Server=localhost;Port=5432;Database=bookishDB;Username=bookish;Password=softwire";

        public IEnumerable<Customer> GetAllCustomers()
        {
            var getCustomersQuery =
                "SELECT id as CustomerId, firstname, lastname, address  from customer";

            using var connection = new NpgsqlConnection(ConnectionString);

            return connection.Query<Customer>(getCustomersQuery);
        }
        
        public Customer GetCustomer(int userId)
        {
            var searchParameters = new DynamicParameters(new {UserId = userId});
            var getCustomersQuery =
                "SELECT id as CustomerId, firstname, lastname, address from customer WHERE (id = @UserId)";

            using var connection = new NpgsqlConnection(ConnectionString);

            return connection.Query<Customer>(getCustomersQuery, searchParameters).First();
        }

        public int CreateUser(CreateCustomerModel newUser)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                var sqlStatement =
                    @"INSERT INTO customer (firstname, lastname, address) VALUES (@FirstName, @LastName, @Address) RETURNING id";
                var result = connection.QuerySingle<int>(sqlStatement, newUser);
                return result;
            }
        }

        public async void DeleteUser(DeleteCustomerModel user)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                var sqlStatement =
                    @"DELETE FROM customer where id = @CustomerId";
                await connection.OpenAsync();
                await connection.ExecuteAsync(sqlStatement, user);
            }
        }
        
        public async void EditUserDetails(Customer user)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                var sqlStatement =
                    @"UPDATE customer
                        SET firstname = @FirstName,
	                        lastname = @LastName,
	                        address = @Address
                        WHERE id = @CustomerId;";
                await connection.OpenAsync();
                await connection.ExecuteAsync(sqlStatement, user);
            }
        }
    }
}