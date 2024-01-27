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
            //var orderStatus= await _orderService.CreateOrder(checkoutInfoInput);
            //if (!orderStatus.IsSuccess)
            //{
            //    var basket= await _basketService.Get();
            //    ViewBag.basket = basket;
            //    ViewBag.error = orderStatus.Error;
            //    TempData["error"] = orderStatus.Error;
            //    return RedirectToAction(nameof(Checkout));
            //    return View(); //Kodlanacak.
            //}
            //return RedirectToAction(nameof(SuccessfullCheckout), new { orderId = orderStatus.OrderId });

            var orderSuspend = await _orderService.SuspendOrder(checkoutInfoInput);
            if (!orderSuspend.IsSuccessful)
            {
                var basket = await _basketService.Get();

                ViewBag.basket = basket;

                ViewBag.error = orderSuspend.Error;

                return View();
            }
            return RedirectToAction(nameof(SuccessfullCheckout), new { orderId = new Random().Next(1, 1000) });
        }
        public IActionResult SuccessfullCheckout(int orderId)
        {
            ViewBag.orderId = orderId;
            return View();
        }



        public async Task<IActionResult> CheckoutHistory()
        {
            return View(await _orderService.GetOrder());
        }
    }
}
