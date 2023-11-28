using Web.Models.Orders;

namespace Web.Services.Interfaces;

public interface IOrderService
{
    /// <summary>
    /// Senkron / synchronous direkt olarak order microservice'ine istek yapılacak.
    /// </summary>
    /// <param name="checkoutInfoInput"></param>
    /// <returns></returns>
    Task<OrderCreatedViewModel> CreateOrder(CheckoutInfoInput checkoutInfoInput);

    /// <summary>
    /// Async Sipariş bilgileri rabbitmq ya gönderilecek
    /// </summary>
    /// <param name="checkoutInfoInput"></param>
    /// <returns></returns> 
    Task SuspendOrder(CheckoutInfoInput checkoutInfoInput);

    Task <List<OrderViewModel>> GetOrder();
}
