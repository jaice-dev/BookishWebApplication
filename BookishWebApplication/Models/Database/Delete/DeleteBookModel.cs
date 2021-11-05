using System.ComponentModel.DataAnnotations;

namespace BookishWebApplication.Models.Database.Delete
{
    public class DeleteBookModel
    {
        [Required]
        public int BookId { get; set; }

    }
}