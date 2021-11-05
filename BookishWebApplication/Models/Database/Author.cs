using System.Collections.Generic;

namespace BookishWebApplication.Models.Database
{
    public class Author : IEqualityComparer<Author>
    {
        public int AuthorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => FirstName + " " + LastName;
        
        public bool Equals (Author x, Author y)
        {
            if (x == null || y == null) return false;

            return (x.AuthorId == y.AuthorId);
        }

        public int GetHashCode(Author obj)
        {
            return AuthorId.GetHashCode();
        }
    }
}