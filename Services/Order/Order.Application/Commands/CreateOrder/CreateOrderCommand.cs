using MediatR;
using Order.Application.Dtos;
using SharedLibrary.Dtos;

namespace Order.Application.Commands.CreateOrder
{
    public class CreateOrderCommand:IRequest<Response<CreatedOrderDto>>
    {
        public string UserId { get; set; }
        public List<OrderItemDto>? OrderItems { get; set; }
        public AddressDto Address { get; set; }
    }
}