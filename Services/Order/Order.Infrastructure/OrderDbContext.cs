using Microsoft.EntityFrameworkCore;

namespace Order.Infrastructure;

public class OrderDbContext:DbContext
{
    public const string DEFAULT_SCHEMA = "ordering"; //Bu Scheme neden belirlendi.?
    public OrderDbContext(DbContextOptions<OrderDbContext> dbContextOptions) :base(dbContextOptions)
    {
        //Base e options'u yolladık.
    }
    DbSet<Domain.OrderAggregate.Order> Orders { get; set; }
    DbSet<Domain.OrderAggregate.OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Domain.OrderAggregate.Order>().ToTable("Orders", DEFAULT_SCHEMA);
        modelBuilder.Entity<Domain.OrderAggregate.OrderItem>().ToTable("Orders", DEFAULT_SCHEMA);
        base.OnModelCreating(modelBuilder);
    }
}