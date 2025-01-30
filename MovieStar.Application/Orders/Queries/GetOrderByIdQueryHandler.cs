using MediatR;
using MovieStar.Application.DTOs;
using MovieStar.Domain.Interfaces;

namespace MovieStar.Application.Orders.Queries
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDto>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrderByIdQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrderByIdAsync(request.OrderId);

            if (order == null)
            {
                return null;
            }

            return new OrderDto
            {
                Id = order.Id,
                CustomerEmail = order.Customer.Email,
                CustomerName = order.Customer.FullName,
                OrderDate = order.OrderDate,
                Total = order.Total,
                OrderProducts = order.OrderProducts.Select(op => new OrderProductDto
                {
                    ProductName = op.Product.Name,
                    Quantity = op.Quantity,
                    Price = (decimal)op.Product.Price
                }).ToList()
            };
        }

    }
}
