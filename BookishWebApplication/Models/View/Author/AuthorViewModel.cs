using System.Collections;
using System.Collections.Generic;
using BookishWebApplication.Models.Database;

namespace BookishWebApplication.Models.View
{
    public class AuthorViewModel
    {
        public Author Author { get; set; }
        public IEnumerable<Book> BooksByAuthor { get; set; }
    }
}