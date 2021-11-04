using System.ComponentModel.DataAnnotations;

namespace BookishWebApplication.Models.Database.Create
{
    public class CreateBookAuthorModel
    {
        [Required]
        public string Title { get; set; }
        public short PublicationYear { get; set; }
        public string Isbn { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}