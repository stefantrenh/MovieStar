namespace MovieStar.Application.Customers.DTOs
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool PremiumMembership { get; set; }
    }
}
