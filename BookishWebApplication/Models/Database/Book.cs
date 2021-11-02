using System;

namespace BookishWebApplication.Models.Database
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public short PublicationYear { get; set; }
        public string Isbn { get; set; }
    }
}