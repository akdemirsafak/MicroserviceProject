namespace Order.Application.Dtos;

public class OrderDto{
    public int Id { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public AddressDto Address { get; set; } //[Owned] olarak dbcontext'de işaretleyeceğiz.
    public string BuyerId { get; set; }
    public List<OrderItemDto> OrderItems { get ; set; }
}