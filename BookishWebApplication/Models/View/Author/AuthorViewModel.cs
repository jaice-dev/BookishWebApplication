using System.Collections.Generic;
using BookishWebApplication.Models.Database;
using BookishWebApplication.Models.Database.Create;

namespace BookishWebApplication.Models.View
{
    public class AuthorViewModel
    {
        public Author Author { get; set; }
        public IEnumerable<Book> AllBooks { get; set; }
        public BookAuthor BookAuthor { get; set; }
        public IEnumerable<Book> BooksByAuthor { get; set; }
    }
}