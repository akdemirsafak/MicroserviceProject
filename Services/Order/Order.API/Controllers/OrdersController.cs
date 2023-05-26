using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Commands.CreateOrder;
using Order.Application.Queries.GetOrdersByUserId;
using SharedLibrary.ControllerBases;
using SharedLibrary.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : CustomBaseController
    {
        private readonly IMediator _mediator;
        private readonly ISharedIdentityService _sharedIdentityService;

        public OrdersController(IMediator mediator, ISharedIdentityService sharedIdentityService)
        {
            _mediator = mediator;
            _sharedIdentityService = sharedIdentityService;
        }

        // GET: api/<OrdersController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _mediator.Send(new GetOrdersByUserIdQuery { UserId=_sharedIdentityService.GetUserId });
            return CreateActionResult(response);
        }

        // GET api/<OrdersController>/5
        [HttpPost]
        public async Task<IActionResult> SaveOrder(CreateOrderCommand request)
        {
            var response = await _mediator.Send(request);
            return CreateActionResult(response);
        }

    }
}
