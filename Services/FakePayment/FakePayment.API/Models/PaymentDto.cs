namespace FakePayment.API.Models;

public class PaymentDto
{
    public string CardName { get; set; }
    public string CardNumber { get; set; }
    public string ExpirationDate { get; set; }
    public string CVV { get; set; }
    public decimal TotalPrice { get; set; }
}
