using MovieStar.Domain.Entities;

namespace MovieStar.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer> GetByEmailAsync(string email);
        Task CreateAsync(Customer customer);
    }
}
