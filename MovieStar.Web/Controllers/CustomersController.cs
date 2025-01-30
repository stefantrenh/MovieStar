using MediatR;
using Microsoft.AspNetCore.Mvc;
using MovieStar.Application.Customers.Commands;
using MovieStar.Application.Customers.Queries;
using MovieStar.Web.Models;

namespace MovieStar.Web.Controllers
{
    public class CustomersController : Controller
    {
        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public IActionResult AddCustomer()
        {
            return View(new AddCustomerViewModel());
        }


        [HttpPost]
        public async Task<IActionResult> AddCustomer(AddCustomerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
        
            var command = new AddCustomerCommand
            {
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                PremiumMembership = model.PremiumMembership
            };
        
            var result = await _mediator.Send(command);
        
            if (result == -1)
            {
                ModelState.AddModelError("Email", "A customer with this email already exists.");
                return View(model);
            }
        
            TempData["Success"] = "Customer added successfully!";
            return RedirectToAction("CustomerList");
        }


        [HttpGet]
        public async Task<IActionResult> CustomerList()
        {
            var customers = await _mediator.Send(new GetAllCustomersQuery());
            return View(customers);
        }

    }
}
