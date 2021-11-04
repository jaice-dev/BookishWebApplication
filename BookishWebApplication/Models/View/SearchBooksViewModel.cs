using System.Collections.Generic;
using BookishWebApplication.Models.Database;
using BookishWebApplication.Models.Database.Create;

namespace BookishWebApplication.Models.View
{
    public class SearchBooksViewModel : BooksViewModel
    {
        public string SearchString { get; set; }
        public IEnumerable<Book> Books { get; set; }
        public IEnumerable<Author> Authors { get; set; }
        public IEnumerable<BookAuthor> BookAuthors { get; set; }
    }
}