using System.ComponentModel.DataAnnotations;

namespace BookishWebApplication.Models.Database.Create
{
    public class CreateCustomerModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        
        public string Address { get; set; }

    }
}