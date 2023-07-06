using Basket.API.Dtos;
using Basket.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.ControllerBases;
using SharedLibrary.Services;

namespace Basket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : CustomBaseController
    {
        private readonly IBasketService _basketService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public BasketsController(IBasketService basketService, ISharedIdentityService sharedIdentityService)
        {
            _basketService = basketService;
            _sharedIdentityService = sharedIdentityService;
        }
        [HttpGet]
        public async Task<IActionResult> GetBasket()
        {
            //Jwt'nin payload'ında gelen user bilgilerini alalım:
            //var claims=HttpContext.User.Claims;
            var claims = User.Claims;
            return CreateActionResult(await _basketService.GetBasket(_sharedIdentityService.GetUserId));
        }

        [HttpPost]
        public async Task<IActionResult> SaveOrUpdateBasket(BasketDto basketDto)
        {
            basketDto.UserId = _sharedIdentityService.GetUserId;
            var response = await _basketService.SaveOrUpdate(basketDto);
            return CreateActionResult(response);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteBasket()
        {
            return CreateActionResult(await _basketService.Delete(_sharedIdentityService.GetUserId));
        }
    }
}
