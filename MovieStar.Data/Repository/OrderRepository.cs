using Microsoft.EntityFrameworkCore;
using MovieStar.Data.MovieStar.Data;
using MovieStar.Domain.Entities;
using MovieStar.Domain.Interfaces;
using MovieStar.Domain.Enums;

namespace MovieStar.Data.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Customer> GetCustomerByEmailAsync(string email)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<Product> GetProductByFormatAsync(VideoFormatType formatType, bool premiumMember)
        {
            return await _context.Products.Where(p => p.FormatType == formatType && p.Discount == premiumMember)
                            .FirstOrDefaultAsync();
        }

        public async Task<int> CreateOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order.Id;
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }
    }
}
