using MassTransit;
using Order.Domain.OrderAggregate;
using Order.Infrastructure;
using SharedLibrary.Messages;

namespace Order.Application.Consumers;

public class CreateOrderMessageCommandConsumer : IConsumer<CreateOrderMessageCommand>
{
    private readonly OrderDbContext _dbContext;

    public CreateOrderMessageCommandConsumer(OrderDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<CreateOrderMessageCommand> context)
    {
        var newAddress= new Address(context.Message.Province, context.Message.District,context.Message.Street,context.Message.Line,context.Message.ZipCode);

        var newOrder = new Domain.OrderAggregate.Order(newAddress,context.Message.BuyerId);

        context.Message.OrderItems.ForEach(x =>
        {
            newOrder.AddOrderItem(x.ProductId,x.ProductName,x.Price,x.PictureUrl);
        });

        await _dbContext.Orders.AddAsync(newOrder);
        await _dbContext.SaveChangesAsync();
    }
}
