using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Dtos;

namespace SharedLibrary.ControllerBases
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBaseController : ControllerBase
    {
        [NonAction] 
        public IActionResult CreateActionResult<T>(Response<T> response) 
        {
            return new ObjectResult(response) 
            { 
                StatusCode=response.StatusCode,

            };
        }
    }
}
