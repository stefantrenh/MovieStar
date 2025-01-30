using FluentAssertions;
using Moq;
using MovieStar.Application.Orders.Commands;
using MovieStar.Domain.Entities;
using MovieStar.Domain.Enums;
using MovieStar.Domain.Interfaces;


namespace MovieStar.UnitTest.OrderTests
{
    public class CreateOrderCommandHandlerTest
    {
        protected readonly Mock<IOrderRepository> _orderRepositoryMock;
        protected readonly CreateOrderCommandHandler _handler;
        protected readonly Customer _customer;
        protected readonly Product _dvdProduct;
        protected readonly Product _specialOfferProduct;
        protected readonly Product _blueRayProduct;
        protected CreateOrderCommand _command;

        public CreateOrderCommandHandlerTest()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _handler = new CreateOrderCommandHandler(_orderRepositoryMock.Object);

            _customer = new Customer
            {
                Email = "stefan@hotmail.com",
                PremiumMembership = true
            };

            _dvdProduct = new Product { FormatType = VideoFormatType.DVD, Price = 29 };
            _blueRayProduct = new Product { FormatType = VideoFormatType.BlueRay, Price = 39 };
            _specialOfferProduct = new Product { FormatType = VideoFormatType.Other, Price = 100 };
            _command = new CreateOrderCommand { Email = _customer.Email, QuantityDVD = 5, QuantityBluRay = 5 };

            _orderRepositoryMock.Setup(repo => repo.GetCustomerByEmailAsync(_customer.Email)).ReturnsAsync(_customer);
            _orderRepositoryMock.Setup(repo => repo.GetProductByFormatAsync(VideoFormatType.DVD, _customer.PremiumMembership)).ReturnsAsync(_dvdProduct);
            _orderRepositoryMock.Setup(repo => repo.GetProductByFormatAsync(VideoFormatType.BlueRay, _customer.PremiumMembership)).ReturnsAsync(_blueRayProduct);
            _orderRepositoryMock.Setup(repo => repo.GetProductByFormatAsync(VideoFormatType.Other, _customer.PremiumMembership)).ReturnsAsync(_specialOfferProduct);
            _orderRepositoryMock.Setup(repo => repo.CreateOrderAsync(It.IsAny<Order>())).ReturnsAsync(1);
        }
    }

    public class MixedProductsTest : CreateOrderCommandHandlerTest
    {
        [Fact]
        public async Task ShouldCalculateTwoSpecialsOffersAndTwoBlueRays()
        {
            Order order = null;
            _orderRepositoryMock.Setup(repo => repo.CreateOrderAsync(It.IsAny<Order>()))
                .Callback<Order>(o => order = o)
                .ReturnsAsync(1);

            var result = await _handler.Handle(_command, CancellationToken.None);

            _orderRepositoryMock.Verify(repo => repo.CreateOrderAsync(It.IsAny<Order>()), Times.Once);

            order.Should().NotBeNull();
            order.OrderProducts.Should().HaveCount(2); 

            order.OrderProducts.Should().ContainSingle(op => op.Product.FormatType == VideoFormatType.Other && op.Quantity == 2);
            order.OrderProducts.Should().ContainSingle(op => op.Product.FormatType == VideoFormatType.BlueRay && op.Quantity == 2);

            decimal bluRayPrice = _blueRayProduct.Price;
            decimal specialOfferPrice = _specialOfferProduct.Price;
            decimal expectedTotalPrice = (2 * bluRayPrice) + (2 * specialOfferPrice);

            order.Total.Should().Be(expectedTotalPrice);
        }
    }

    public class FiveDvdTest : CreateOrderCommandHandlerTest
    {
        [Fact]
        public async Task ShouldCalculateOneSpecialOfferAndOneDvd()
        {
            _command = new CreateOrderCommand { Email = _customer.Email, QuantityDVD = 5 };

            Order order = null;
            _orderRepositoryMock.Setup(repo => repo.CreateOrderAsync(It.IsAny<Order>()))
                .Callback<Order>(o => order = o)
                .ReturnsAsync(1);

            var result = await _handler.Handle(_command, CancellationToken.None);

            _orderRepositoryMock.Verify(repo => repo.CreateOrderAsync(It.IsAny<Order>()), Times.Once);

            order.Should().NotBeNull();
            order.OrderProducts.Should().HaveCount(2);

            order.OrderProducts.Should().ContainSingle(op => op.Product.FormatType == VideoFormatType.Other && op.Quantity == 1);
            order.OrderProducts.Should().ContainSingle(op => op.Product.FormatType == VideoFormatType.DVD && op.Quantity == 1);

            decimal dvdPrice = _dvdProduct.Price;
            decimal specialOfferPrice = _specialOfferProduct.Price;
            decimal expectedTotalPrice = (1 * dvdPrice) + (1 * specialOfferPrice);

            order.Total.Should().Be(expectedTotalPrice);
        }
    }

    public class FiveBlueRayTest : CreateOrderCommandHandlerTest
    {
        [Fact]
        public async Task ShouldCalculateOneSpecialOfferAndOneBlueRay()
        {
            _command = new CreateOrderCommand { Email = _customer.Email, QuantityBluRay = 5 };

            Order order = null;
            _orderRepositoryMock.Setup(repo => repo.CreateOrderAsync(It.IsAny<Order>()))
                .Callback<Order>(o => order = o)
                .ReturnsAsync(1);

            var result = await _handler.Handle(_command, CancellationToken.None);

            _orderRepositoryMock.Verify(repo => repo.CreateOrderAsync(It.IsAny<Order>()), Times.Once);

            order.Should().NotBeNull();
            order.OrderProducts.Should().HaveCount(2);

            order.OrderProducts.Should().ContainSingle(op => op.Product.FormatType == VideoFormatType.Other && op.Quantity == 1);
            order.OrderProducts.Should().ContainSingle(op => op.Product.FormatType == VideoFormatType.BlueRay && op.Quantity == 1);

            decimal blueRayPrice = _blueRayProduct.Price;
            decimal specialOfferPrice = _specialOfferProduct.Price;
            decimal expectedTotalPrice = (1 * blueRayPrice) + (1 * specialOfferPrice);

            order.Total.Should().Be(expectedTotalPrice);
        }
    }

    public class FourBlueRaysAndFourDvdTest : CreateOrderCommandHandlerTest
    {
        [Fact]
        public async Task ShouldCalculateTwoSpecialOrders()
        {
            _command = new CreateOrderCommand { Email = _customer.Email, QuantityBluRay = 4 , QuantityDVD = 4 };

            Order order = null;
            _orderRepositoryMock.Setup(repo => repo.CreateOrderAsync(It.IsAny<Order>()))
                .Callback<Order>(o => order = o)
                .ReturnsAsync(1);

            var result = await _handler.Handle(_command, CancellationToken.None);

            _orderRepositoryMock.Verify(repo => repo.CreateOrderAsync(It.IsAny<Order>()), Times.Once);

            order.Should().NotBeNull();
            order.OrderProducts.Should().HaveCount(1);
            order.OrderProducts.Should().ContainSingle(op => op.Product.FormatType == VideoFormatType.Other && op.Quantity == 2);

            decimal specialOfferPrice = _specialOfferProduct.Price;
            decimal expectedTotalPrice = 2 * specialOfferPrice;

            order.Total.Should().Be(expectedTotalPrice);
        }
    }
    

}
