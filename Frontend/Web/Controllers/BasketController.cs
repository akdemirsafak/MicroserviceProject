using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Models.Baskets;
using Web.Models.Discounts;
using Web.Services.Interfaces;

namespace Web.Controllers
{
    [Authorize]
    public class BasketController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly ICatalogService _catalogService;

        public BasketController(IBasketService basketService, ICatalogService catalogService)
        {
            _basketService = basketService;
            _catalogService = catalogService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _basketService.Get());
        }
        public async Task<IActionResult> AddBasketItem(string courseId)
        {
            var course = await _catalogService.GetByCourseId(courseId);
            var basketItem = new BasketItemViewModel { CourseId = course.Id, CourseName = course.Name, Price = course.Price };
            await _basketService.AddBasketItem(basketItem);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> RemoveBasketItem(string basketItemId)
        {
          var result= await _basketService.RemoveBasketItem(basketItemId);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ApplyDiscount(DiscountApplyInput discountApplyInput)
        {
            if (!ModelState.IsValid)
            {
                TempData["discountErr"] = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).First();
                return RedirectToAction(nameof(Index));
            }
            var discountStatus=await _basketService.ApplyDiscount(discountApplyInput.Code);
            TempData["discountStatus"] = discountStatus;
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> CancelApplyDiscount(DiscountApplyInput discountApplyInput)
        {
            var discountStatus=await _basketService.CancelApplyDiscount();
            TempData["discountStatus"] = discountStatus;
            return RedirectToAction(nameof(Index));
        }
    }
}
