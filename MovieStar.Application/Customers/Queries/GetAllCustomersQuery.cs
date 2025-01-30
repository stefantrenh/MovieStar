using MediatR;
using MovieStar.Application.Customers.DTOs;

namespace MovieStar.Application.Customers.Queries
{
    public class GetAllCustomersQuery : IRequest<List<CustomerDto>>
    {
    }
}
