using System.ComponentModel;

namespace Web.Models.Orders;

public class CheckoutInfoInput
{
    [DisplayName("İl : ")]
    public string Province { get; set; }
    [DisplayName("İlçe : ")]
    public string District { get; set; }
    [DisplayName("Cadde : ")]
    public string Street { get; set; }
    [DisplayName("Posta Kodu : ")]
    public string ZipCode { get; set; }
    [DisplayName("Adres : ")]
    public string Line { get; set; }
    [DisplayName("Kart sahibi adı soyadı : ")]
    public string CardName { get; set; }
    [DisplayName("Kart Numarası : ")]
    public string CardNumber { get; set; }
    [DisplayName("son kullanma tarihi ay/yıl : ")]
    public string ExpirationDate { get; set; }
    [DisplayName("CVV veya CVC : ")]
    public string CVV { get; set; }
    public decimal TotalPrice { get; set; }
}
