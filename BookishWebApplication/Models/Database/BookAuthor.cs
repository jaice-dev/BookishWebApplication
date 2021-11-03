using System;
using System.Collections.Generic;

namespace BookishWebApplication.Models.Database
{
    public class BookAuthor
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int AuthorId { get; set; }
    }
}