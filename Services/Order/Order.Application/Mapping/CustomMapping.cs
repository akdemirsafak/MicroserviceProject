using AutoMapper;
using Order.Application.Dtos;

namespace Order.Application.Mapping;

class CustomMapping : Profile{
    public CustomMapping()
    {
        CreateMap<Order.Domain.OrderAggregate.Order,OrderDto>().ReverseMap(); //Order'ı OrderDto'ya çevir. Tam tersini de reversemap ile sağlayabilir hale getirdik.
        CreateMap<Order.Domain.OrderAggregate.OrderItem,OrderItemDto>().ReverseMap();
        CreateMap<Order.Domain.OrderAggregate.Address,AddressDto>().ReverseMap();;
    }
}