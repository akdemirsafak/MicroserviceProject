using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Order.Infrastructure.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<Domain.OrderAggregate.OrderItem>
{
    public void Configure(EntityTypeBuilder<Domain.OrderAggregate.OrderItem> builder)
    {
        builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
    }
}