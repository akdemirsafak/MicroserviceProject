using MicroserviceProject.IdentityServer.Dtos;
using MicroserviceProject.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using SharedLibrary.Dtos;
using SharedLibrary.ControllerBases;
using Microsoft.AspNetCore.Authorization;
using static IdentityServer4.IdentityServerConstants;

namespace MicroserviceProject.IdentityServer.Controllers {

    [Authorize(LocalApi.PolicyName)] //Claim bazlı yetkilendirme işlemi gerçekleşiyor.
    [Route("/api/[controller]/[action]")]
    [ApiController]
    public class UserController : CustomBaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SignupDto signupDto) 
        {
            var user = new ApplicationUser() {
            UserName=signupDto.UserName,
            Email=signupDto.Email,
            City=signupDto.City};
            var result =await _userManager.CreateAsync(user,signupDto.Password);
            if (!result.Succeeded)
            {
                return CreateActionResult(Response<NoContent>.Fail(result.Errors.Select(x=>x.Description).ToList(),400));
            }
            return CreateActionResult(Response<NoContent>.Success(201));
        }
    }
}
