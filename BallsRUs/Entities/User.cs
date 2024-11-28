using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BallsRUs.Entities
{
    public class User : IdentityUser<Guid>
    {
        public User(string userName) : base(userName) { }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        // Navigation properties
        public virtual Address? Address { get; set; }
    }
}
