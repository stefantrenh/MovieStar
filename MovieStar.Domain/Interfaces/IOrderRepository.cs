using MovieStar.Domain.Entities;
using MovieStar.Domain.Enums;

namespace MovieStar.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<int> CreateOrderAsync(Order order);
        Task<Customer> GetCustomerByEmailAsync(string email);
        Task<Product> GetProductByFormatAsync(VideoFormatType formatType, bool premiumMember);
        Task<Order> GetOrderByIdAsync(int orderId);

    }
}
