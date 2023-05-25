using Microsoft.EntityFrameworkCore;

namespace Order.Infrastructure.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Domain.OrderAggregate.Order>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Domain.OrderAggregate.Order> builder)
    {
        builder.OwnsOne(o => o.Address).WithOwner();
    }
}