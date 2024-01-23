namespace FakePayment.API.Models;

public class PaymentDto
{
    public string CardName { get; set; }
    public string CardNumber { get; set; }
    public string ExpirationDate { get; set; }
    public string CVV { get; set; }
    public decimal TotalPrice { get; set; }
    public OrderDto Order { get; set; }
}
//Ödeme işleme alındıktan sonra sipariş oluşturulması için RabbitMQ'ya sipariş mesajı gönderilmesi gerekiyor.PaymentDto'nun içerisinde sipariş bilgileri de bulunması gerekli.
//Update'imizi bu alanlar için yapıyoruz. 