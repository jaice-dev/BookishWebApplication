using System;
using System.Collections.Generic;

namespace BookishWebApplication.Models.Database
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public short PublicationYear { get; set; }
        public string Isbn { get; set; }
        public int PrintCount { get; set; }
        public List<Author> Authors { get; set; }
    }
}