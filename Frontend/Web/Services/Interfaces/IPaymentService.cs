using Web.Models.Payments;

namespace Web.Services.Interfaces;

public interface IPaymentService
{
    Task<bool> ReceivePayment(PaymentInfoInput paymentInfoInput);
}
