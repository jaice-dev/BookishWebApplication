using System.Collections.Generic;
using BookishWebApplication.Models.Database;

namespace BookishWebApplication.Models.View
{
    public class CustomersViewModel
    {
        public IEnumerable<Customer> AllUsers { get; set; }
        
    }
}