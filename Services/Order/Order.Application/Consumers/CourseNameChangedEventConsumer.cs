using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.Infrastructure;
using SharedLibrary.Messages;

namespace Order.Application.Consumers;

public class CourseNameChangedEventConsumer : IConsumer<CourseNameChangedEvent>
{
    private readonly OrderDbContext _context;

    public CourseNameChangedEventConsumer(OrderDbContext context)
    {
        _context = context;
    }

    public async Task Consume(ConsumeContext<CourseNameChangedEvent> context)
    {
       var orderItems= await _context.OrderItems.Where(x => x.ProductId == context.Message.CourseId).ToListAsync();

        orderItems.ForEach(x =>
        {
            x.UpdateOrderItem(context.Message.UpdatedName, x.PictureUrl, x.Price);
        });
        await _context.SaveChangesAsync();
    }
}
