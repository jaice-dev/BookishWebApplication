using System.ComponentModel.DataAnnotations;

namespace BookishWebApplication.Models.Database.Create
{
    public class DeleteAuthorModel
    {
        [Required]
        public int AuthorId { get; set; }

    }
}