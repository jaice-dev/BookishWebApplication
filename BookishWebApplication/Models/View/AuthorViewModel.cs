using System.Collections;
using System.Collections.Generic;
using BookishWebApplication.Models.Database;

namespace BookishWebApplication.Models.View
{
    public class AuthorViewModel
    {
        public IEnumerable<Author> Authors { get; set; }
    }
}