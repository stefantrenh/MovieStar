using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MovieStar.Application.Customers.Commands
{
    public class AddCustomerCommand : IRequest<int>
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
