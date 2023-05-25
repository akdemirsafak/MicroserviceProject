namespace Order.Application.Dtos;

public class OrderDto{
    public int Id { get; set; }
    public DateTime CreatedDateTime { get; private set; }
    public AddressDto Address { get; private set; } //[Owned] olarak dbcontext'de işaretleyeceğiz.
    public string BuyerId { get;private set; }
    public List<OrderItemDto> OrderItems { get ; set; }
}