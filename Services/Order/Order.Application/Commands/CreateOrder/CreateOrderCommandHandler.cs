using MediatR;
using Order.Application.Dtos;
using Order.Domain.OrderAggregate;
using Order.Infrastructure;
using SharedLibrary.Dtos;

namespace Order.Application.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Response<CreatedOrderDto>>
    {
        private readonly OrderDbContext _dbContext;

        public CreateOrderCommandHandler(OrderDbContext dbContext)=>            _dbContext = dbContext;

        public async Task<Response<CreatedOrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var address=new Address(
                request.Address.Province,
                request.Address.District,
                request.Address.Street,
                request.Address.City,
                request.Address.Line,
                request.Address.ZipCode
            );
            Domain.OrderAggregate.Order newOrder=new(address,request.UserId);

            request.OrderItems.ForEach(orderItem =>{
                newOrder.AddOrderItem(orderItem.ProductId,orderItem.ProductName,orderItem.Price,orderItem.PictureUrl);
            });
            await _dbContext.Orders.AddAsync(newOrder);
            await _dbContext.SaveChangesAsync();
            return Response<CreatedOrderDto>.Success(new CreatedOrderDto{OrderId=newOrder.Id},200);
        }
    }
}