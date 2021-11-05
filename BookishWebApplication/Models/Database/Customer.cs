using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookishWebApplication.Models.Database
{
    public class Customer
    {
        public int CustomerId { get; set; }
        
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        
        public string FullName => FirstName + " " + LastName;
        public string Address { get; set; }
        
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(FirstName);
        }
        
    }
}