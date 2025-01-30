using MediatR;

namespace MovieStar.Application.Orders.Commands
{
    public class CreateOrderCommand : IRequest<int>
    {
        public string Email { get; set; }
        public int QuantityDVD { get; set; }
        public int QuantityBluRay { get; set; }
    }
}
