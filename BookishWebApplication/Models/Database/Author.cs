using System;
using System.Collections.Generic;

namespace BookishWebApplication.Models.Database
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}