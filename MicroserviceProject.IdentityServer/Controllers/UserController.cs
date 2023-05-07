using MicroserviceProject.IdentityServer.Dtos;
using MicroserviceProject.IdentityServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using SharedLibrary.ControllerBases;
using SharedLibrary.Dtos;
using System.Linq;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace MicroserviceProject.IdentityServer.Controllers
{

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
            var user = new ApplicationUser()
            {
                UserName=signupDto.UserName,
                Email=signupDto.Email,
                City=signupDto.City
            };
            var result =await _userManager.CreateAsync(user,signupDto.Password);
            if (!result.Succeeded)
            {
                return CreateActionResult(Response<NoContent>.Fail(result.Errors.Select(x => x.Description).ToList(), 400));
            }
            return CreateActionResult(Response<NoContent>.Success(201));
        }
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var userIdClaim=User.Claims.FirstOrDefault(x=>x.Type==JwtRegisteredClaimNames.Sub);//Bu id token'dan geliyor parametre olarak almamıza gerek yok.
            if (userIdClaim == null)
            {
                return BadRequest();
            }
            var user= await _userManager.FindByIdAsync(userIdClaim.Value);
            if (user == null)
            {
                return BadRequest();
            }
            var userDto= new
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                City = user.City
            };
            return Ok(userDto);
        }
    }
}
