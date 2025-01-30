namespace MovieStar.Application.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerName { get; set; } 
        public DateTime OrderDate { get; set; }
        public decimal Total { get; set; }
        public List<OrderProductDto> OrderProducts { get; set; }
    }
}
