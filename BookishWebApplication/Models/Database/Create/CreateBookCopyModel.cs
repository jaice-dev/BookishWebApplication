using System.ComponentModel.DataAnnotations;

namespace BookishWebApplication.Models.Database.Create
{
    public class CreateBookCopyModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}