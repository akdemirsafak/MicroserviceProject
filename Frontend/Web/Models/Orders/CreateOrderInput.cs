namespace Web.Models.Orders
{
    public class CreateOrderInput
    {
        public CreateOrderInput()
        {
            OrderItems = new List<OrderItemCreateInput>();
        }

        public string UserId { get; set; }
        public List<OrderItemCreateInput>? OrderItems { get; set; }
        public AddressCreateInput Address { get; set; }
    }
}
