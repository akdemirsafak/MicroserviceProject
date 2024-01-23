
using System.Collections.Generic;

namespace SharedLibrary.Messages
{
    
    //Burada tanımlanan alanlar bir siparişin oluşabilmesi için önemli alanlardır.
    public class CreateOrderMessageCommand
    {
        public string BuyerId { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string Line { get; set; }
        
    }

    public class OrderItem
    {
        public string ProductId { get; private set; }
        public string ProductName { get; private set; }
        public string PictureUrl { get; private set; }
        public decimal Price { get; private set;}
    }
    
}