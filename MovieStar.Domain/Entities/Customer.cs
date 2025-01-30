
using System.ComponentModel.DataAnnotations;


namespace MovieStar.Domain.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(15)]
        public string PhoneNumber { get; set; }

        [Required]
        public string Email { get; set; }
        public bool PremiumMembership { get; set; }

        public DateTime Created { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
