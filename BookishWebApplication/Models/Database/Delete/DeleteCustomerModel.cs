using System.ComponentModel.DataAnnotations;

namespace BookishWebApplication.Models.Database.Create
{
    public class DeleteCustomerModel
    {
        [Required]
        public int CustomerId { get; set; }

    }
}