using Discount.API.Services;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.ControllerBases;
using SharedLibrary.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Discount.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : CustomBaseController
    {
        //UserId yi client'ten beklemek yerine Jwt içerisinden alalım.SharedLibrary içerisinde oluşturduğumuz ISharedIdentityService de oluşturduğumuz method sayesinde yapacağız.

        private readonly IDiscountService _discountService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public DiscountsController(IDiscountService discountService, ISharedIdentityService sharedIdentityService)
        {
            _discountService = discountService;
            _sharedIdentityService = sharedIdentityService;
        }

        // GET: api/<DiscountController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return CreateActionResult( await _discountService.GetAll());
        }

        // GET api/<DiscountController>/5
        [HttpGet("{id}")] //From query
        public async Task<IActionResult> GetById(int id)
        {
            return CreateActionResult(await _discountService.GetById(id));
        }
        [HttpGet("GetByCode/{code}")]
        //[Route("/api/[controller]/[action]/{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var userId= _sharedIdentityService.GetUserId;
            return CreateActionResult(await _discountService.GetByCodeAndUserId(code,userId));
        }

        // POST api/<DiscountController>
        [HttpPost]
        public async Task<IActionResult> Save([FromBody] DiscountModel discountModel)
        {
            return CreateActionResult(await _discountService.Save(discountModel));
        }

        
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] DiscountModel discountModel)
        {
            return CreateActionResult(await _discountService.Update(discountModel));
        }

        // DELETE api/<DiscountController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return CreateActionResult(await _discountService.Delete(id));
        }
    }
}
