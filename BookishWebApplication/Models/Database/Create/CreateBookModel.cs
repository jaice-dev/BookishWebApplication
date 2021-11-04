using System.ComponentModel.DataAnnotations;

namespace BookishWebApplication.Models.Database.Create
{
    public class CreateBookModel
    {
        [Required]
        public string Title { get; set; }
        public short? PublicationYear { get; set; }
        public string Isbn { get; set; }
    }
}