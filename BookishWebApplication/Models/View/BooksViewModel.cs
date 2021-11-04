using System.Collections.Generic;
using BookishWebApplication.Models.Database;

namespace BookishWebApplication.Models.View
{
    public class BooksViewModel
    {
        public IEnumerable<Book> Books { get; set; }
    }
}