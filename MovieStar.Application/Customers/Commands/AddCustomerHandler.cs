using MediatR;
using Microsoft.EntityFrameworkCore;
using MovieStar.Data.MovieStar.Data;
using MovieStar.Domain.Entities;

namespace MovieStar.Application.Customers.Commands
{
    public class AddCustomerCommandHandler : IRequestHandler<AddCustomerCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public AddCustomerCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
        {
            var existingCustomer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Email == request.Email, cancellationToken);

            if (existingCustomer is not null)
            {
                return -1;
            }

            var customer = new Customer
            {
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                PremiumMembership = request.PremiumMembership,
                Created = DateTime.UtcNow
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync(cancellationToken);

            return customer.Id;
        }
    }
}
