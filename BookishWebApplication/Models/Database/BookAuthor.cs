using System.ComponentModel.DataAnnotations;

namespace BookishWebApplication.Models.Database
{
    public class BookAuthor
    {
        [Required]
        public int BookId { get; set; }
        [Required]
        public int AuthorId { get; set; }
    }
}