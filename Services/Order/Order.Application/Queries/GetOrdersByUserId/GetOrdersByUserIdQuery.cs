using MediatR;
using Order.Application.Dtos;
using SharedLibrary.Dtos;

namespace Order.Application.Queries.GetOrdersByUserId;
    
    public class GetOrdersByUserIdQuery: IRequest<Response<List<OrderDto>>>
    {
        public string UserId { get; set; }
    }