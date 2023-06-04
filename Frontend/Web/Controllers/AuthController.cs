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
                return View(signInInput);
            }

            var response= await _identityService.SignInAsync(signInInput);
            if (!response.IsSuccessfull)
            {
                response.Errors.ForEach(error => { ModelState.AddModelError(String.Empty, error); });
            }

            return RedirectToAction(nameof(Index), "Home");

            
        }
    }
}
