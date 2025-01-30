using System.ComponentModel.DataAnnotations;

namespace MovieStar.Web.Models
{
    public class AddCustomerViewModel
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(15)]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public bool PremiumMembership { get; set; }
    }
}
