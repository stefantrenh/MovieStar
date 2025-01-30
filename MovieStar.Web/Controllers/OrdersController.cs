using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieStar.Application.Orders.Commands;
using MovieStar.Application.Orders.Queries;
using MovieStar.Web.Models;

namespace MovieStar.Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public IActionResult Create()
        {
            return View(new CreateOrderViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var command = new CreateOrderCommand
            {
                Email = model.Email,
                QuantityDVD = model.QuantityDVD,
                QuantityBluRay = model.QuantityBluRay
            };

            var result = await _mediator.Send(command);

            if (result == -1)
            {
                ModelState.AddModelError("", "You must order at least one product.");
                return View(model);
            }
            if (result == -2)
            {
                ModelState.AddModelError("", "Customer not found.");
                return View(model);
            }
            if (result == -3)
            {
                ModelState.AddModelError("", "Selected products are not available.");
                return View(model);
            }

            return RedirectToAction("OrderDetails", new { orderId = result });

        }

        [HttpGet]
        public async Task<IActionResult> OrderDetails(int orderId)
        {
            var order = await _mediator.Send(new GetOrderByIdQuery(orderId));

            if (order is null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}
