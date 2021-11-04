using System.Collections.Generic;
using BookishWebApplication.Models.Database;
using BookishWebApplication.Models.Database.Create;

namespace BookishWebApplication.Models.View
{
    public class BooksViewModel
    {
        public IEnumerable<Book> Books { get; set; }
        public IEnumerable<Author> AllAuthors { get; set; }
    }
}