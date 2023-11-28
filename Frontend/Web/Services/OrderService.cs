using SharedLibrary.Dtos;
using SharedLibrary.Services;
using System.Net.Http.Json;
using Web.Models.Orders;
using Web.Models.Payments;
using Web.Services.Interfaces;

namespace Web.Services
{
    public class OrderService : IOrderService
    {
        private readonly IPaymentService _paymentService;
        private readonly HttpClient _httpClient;
        private readonly IBasketService _basketService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public OrderService(IPaymentService paymentService, HttpClient httpClient, IBasketService basketService, ISharedIdentityService sharedIdentityService)
        {
            _paymentService = paymentService;
            _httpClient = httpClient;
            _basketService = basketService;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<OrderCreatedViewModel> CreateOrder(CheckoutInfoInput checkoutInfoInput)
        {
            var basket= await _basketService.Get();
            var paymentInfoInput= new PaymentInfoInput()
            {
                CardName = checkoutInfoInput.CardName,
                CardNumber = checkoutInfoInput.CardNumber,
                CVV = checkoutInfoInput.CVV,
                ExpirationDate = checkoutInfoInput.ExpirationDate,
                TotalPrice = basket.TotalPrice
            };
            var responsePayment= await _paymentService.ReceivePayment(paymentInfoInput);

            if (!responsePayment)
            {
                return new OrderCreatedViewModel() { Error = "Ödeme alınamadı.",IsSuccess=false };
            }
            var orderCreateInput = new CreateOrderInput()
            {
                UserId=_sharedIdentityService.GetUserId,
                Address=new AddressCreateInput() {
                    District=checkoutInfoInput.District,
                    Line=checkoutInfoInput.Line,
                    Province=checkoutInfoInput.Province,
                    Street=checkoutInfoInput.Street,
                    ZipCode = checkoutInfoInput.ZipCode
                },
            };

            basket.BasketItems.ForEach(basketItem =>
            {
                orderCreateInput.OrderItems.Add(new OrderItemCreateInput()
                {
                    Price=basketItem.Price,
                    ProductId=basketItem.CourseId,
                    ProductName=basketItem.CourseName,
                    PictureUrl=""
                });
            });
            var response= await _httpClient.PostAsJsonAsync<CreateOrderInput>("orders",orderCreateInput,CancellationToken.None);
            if (response.IsSuccessStatusCode)
            {
                return new OrderCreatedViewModel() { Error = "Sipariş oluşturulamadı.", IsSuccess = false };
            }
            var orderCreatedViewModel= await response.Content.ReadFromJsonAsync<OrderCreatedViewModel>();
            return orderCreatedViewModel;
            
        }

        public async Task<List<OrderViewModel>> GetOrder()
        {
            var response= await _httpClient.GetFromJsonAsync<Response<List<OrderViewModel>>>("orders");
            return response.Data;
        }

        public Task SuspendOrder(CheckoutInfoInput checkoutInfoInput)
        {
            throw new NotImplementedException();
        }
    }
}
