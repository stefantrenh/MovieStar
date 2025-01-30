using System.ComponentModel.DataAnnotations;

namespace MovieStar.Web.Models
{
    public class CreateOrderViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int QuantityDVD { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int QuantityBluRay { get; set; }
    }

}
