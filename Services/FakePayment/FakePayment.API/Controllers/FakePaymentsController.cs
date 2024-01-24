using FakePayment.API.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.ControllerBases;
using SharedLibrary.Dtos;
using SharedLibrary.Messages;

namespace FakePayment.API.Controllers
{
    public class FakePaymentsController : CustomBaseController
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public FakePaymentsController(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }

        [HttpPost]
        public async Task<IActionResult> RecievePayment(PaymentDto paymentDto)
        {
            var sendEndpoint=await _sendEndpointProvider.GetSendEndpoint(new System.Uri("queue:create-order-service"));

            var createOrderMessageCommand = new CreateOrderMessageCommand();
                createOrderMessageCommand.BuyerId = paymentDto.Order.BuyerId;
                createOrderMessageCommand.Province = paymentDto.Order.Address.Province;
                createOrderMessageCommand.District = paymentDto.Order.Address.District;
                createOrderMessageCommand.Street = paymentDto.Order.Address.Street;
                createOrderMessageCommand.ZipCode = paymentDto.Order.Address.ZipCode;
                createOrderMessageCommand.Line = paymentDto.Order.Address.Line;

                paymentDto.Order.OrderItems.ForEach(orderItem =>
                {
                    createOrderMessageCommand.OrderItems.Add(new OrderItem()
                    {
                        ProductId = orderItem.ProductId,
                        ProductName = orderItem.ProductName,
                        PictureUrl = orderItem.PictureUrl,
                        Price = orderItem.Price
                    });
                });
                
            await _sendEndpointProvider.Send<CreateOrderMessageCommand>(createOrderMessageCommand);
                
            
            return CreateActionResult(SharedLibrary.Dtos.Response<NoContent>.Success(200));
        }
    }
}
