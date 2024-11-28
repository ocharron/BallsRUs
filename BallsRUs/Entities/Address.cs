using System.ComponentModel.DataAnnotations.Schema;

namespace BallsRUs.Entities
{
    public class Address
    {
        public Guid Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string StateProvince { get; set; }

        // Foreign keys
        public Guid? UserId { get; set; }

        // Navigation properties
        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }
    }
}
