using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Web.Services.Interfaces;

namespace Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IIdentityService _identityService;

        public AuthController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInInput signInInput)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var response= await _identityService.SignInAsync(signInInput);
            if (!response.IsSuccessfull)
            {
                response.Errors.ForEach(error => { ModelState.AddModelError(String.Empty, error); });
                return View();
            }

            return RedirectToAction(nameof(Index), "User");

        }
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); //Cookie'leri siliyoruz.
            await _identityService.RevokeRefreshToken();
            return RedirectToAction(nameof(HomeController.Index),"Home");
        }
    }
}
