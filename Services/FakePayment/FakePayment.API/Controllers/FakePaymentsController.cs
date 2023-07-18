using FakePayment.API.Models;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.ControllerBases;
using SharedLibrary.Dtos;

namespace FakePayment.API.Controllers
{
    public class FakePaymentsController : CustomBaseController
    {
        [HttpPost]
        public IActionResult RecievePayment(PaymentDto paymentDto)
        {
            return CreateActionResult(Response<NoContent>.Success(200));
        }
    }
}
