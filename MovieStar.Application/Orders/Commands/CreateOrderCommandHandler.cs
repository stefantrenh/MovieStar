using MediatR;
using MovieStar.Domain.Entities;
using MovieStar.Domain.Interfaces;
using MovieStar.Domain.Enums;

namespace MovieStar.Application.Orders.Commands
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepository;

        public CreateOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            if (request.QuantityDVD == 0 && request.QuantityBluRay == 0)
            {
                return -1; 
            }

            var customer = await _orderRepository.GetCustomerByEmailAsync(request.Email);

            if (customer is null)
            {
                return -2; 
            }

            var order = new Order
            {
                Customer = customer,
                OrderDate = DateTime.UtcNow,
                OrderProducts = new List<OrderProduct>()
            };

            if (request.QuantityDVD > 0)
            {
                var dvdProduct = await _orderRepository.GetProductByFormatAsync(VideoFormatType.DVD, customer.PremiumMembership);
                if (dvdProduct is not null)
                {
                    order.OrderProducts.Add(new OrderProduct
                    {
                        Product = dvdProduct,
                        Quantity = request.QuantityDVD
                    });
                }
            }

            if (request.QuantityBluRay > 0)
            {
                var bluRayProduct = await _orderRepository.GetProductByFormatAsync(VideoFormatType.BlueRay, customer.PremiumMembership);
                if (bluRayProduct is not null)
                {
                    order.OrderProducts.Add(new OrderProduct
                    {
                        Product = bluRayProduct,
                        Quantity = request.QuantityBluRay
                    });
                }
            }


            order.Total = await CalculateTotalPrice(order, customer);

            return await _orderRepository.CreateOrderAsync(order); 
        }

        private async Task<decimal> CalculateTotalPrice(Order order, Customer customer)
        {
            if (customer.PremiumMembership)
            {
                var orderList = order.OrderProducts.ToList();

                var specialOfferProduct = await _orderRepository
                    .GetProductByFormatAsync(VideoFormatType.Other, customer.PremiumMembership);

                var totalProducts = order.OrderProducts.Sum(op => op.Quantity);
                var specialOfferCount = totalProducts / 4;
                var remainingProducts = totalProducts % 4;

                var finalOrderProducts = new List<OrderProduct>();
                var productList = new List<Product>();

                foreach (var orderProduct in orderList)
                {
                    for (int i = 0; i < orderProduct.Quantity; i++)
                    {
                        productList.Add(orderProduct.Product);
                    }
                }

                productList.OrderBy(x => x.Price).ToList();

                if (specialOfferCount > 0)
                {
                    finalOrderProducts.Add(new OrderProduct
                    {
                        Product = specialOfferProduct,
                        Quantity = specialOfferCount
                    });
                }

                if (remainingProducts > 0)
                {

                    var remainingProductList = productList.TakeLast(remainingProducts);

                    //Consider of refactor with DRY, but i think its more readble by this way. 
                    if (remainingProductList.Any(ft => ft.FormatType == VideoFormatType.DVD))
                    {
                        finalOrderProducts.Add(new OrderProduct
                        {
                            Product = remainingProductList.First(ft => ft.FormatType == VideoFormatType.DVD),
                            Quantity = remainingProductList.Count(q => q.FormatType == VideoFormatType.DVD)
                        });
                    }

                    if (remainingProductList.Any(ft => ft.FormatType == VideoFormatType.BlueRay))
                    {
                        finalOrderProducts.Add(new OrderProduct
                        {
                            Product = remainingProductList.First(ft => ft.FormatType == VideoFormatType.BlueRay),
                            Quantity = remainingProductList.Count(q => q.FormatType == VideoFormatType.BlueRay)
                        });
                    }
                }

                order.OrderProducts = finalOrderProducts;

                return order.OrderProducts.Sum(op => op.Quantity * op.Product.Price);
            }

            return order.OrderProducts.Sum(op => op.Quantity * op.Product.Price);
        }
    }
}
