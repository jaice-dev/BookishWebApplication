using System.Collections.Generic;
using BookishWebApplication.Models.Database;
using BookishWebApplication.Models.Database.Create;

namespace BookishWebApplication.Models.View
{
    public class BookViewModel
    {
        public Book Book { get; set; }
        public IEnumerable<Author> AllAuthors { get; set; }
        public BookAuthor BookAuthor { get; set; }
        public CreateBookCopyModel CreateCopy { get; set; }

    }
}