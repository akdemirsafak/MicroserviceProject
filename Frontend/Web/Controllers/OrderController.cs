using Microsoft.AspNetCore.Mvc;
using Web.Models.Orders;
using Web.Services.Interfaces;

namespace Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;

        public OrderController(IBasketService basketService, IOrderService orderService)
        {
            _basketService = basketService;
            _orderService = orderService;
        }

        public async Task<IActionResult> Checkout()
        {
            var basket= await _basketService.Get();
            ViewBag.basket = basket;

            return View(new CheckoutInfoInput());
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutInfoInput checkoutInfoInput)
        {
            var orderStatus= await _orderService.CreateOrder(checkoutInfoInput);
            if (!orderStatus.IsSuccess)
            {
                var basket= await _basketService.Get();
                ViewBag.basket = basket;
                ViewBag.error = orderStatus.Error;
                //TempData["error"] = orderStatus.Error;
                //return RedirectToAction(nameof(Checkout));
                return RedirectToAction(nameof(SuccessfullCheckout), new { orderId = orderStatus.OrderId });
            }

            return View(); //Kodlanacak.
        }
        public IActionResult SuccessfullCheckout(int orderId)
        {
            ViewBag.orderId = orderId;
            return View();
        }
    }
}
