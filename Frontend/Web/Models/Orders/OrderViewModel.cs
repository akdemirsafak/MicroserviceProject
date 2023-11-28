﻿namespace Web.Models.Orders
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public DateTime CreatedDateTime { get; set; }
        //public AddressDto Address { get; set; } 
        public string BuyerId { get; set; }
        public List<OrderItemViewModel> OrderItems { get; set; }
    }
}
