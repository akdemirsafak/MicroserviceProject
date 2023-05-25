using Order.Domain.Core;

namespace Order.Domain.OrderAggregate;

//EF Core Features
//-- Owned Types
//-- Shadow Property
//-- Backing Field
public class Order :Entity,IAggregateRoot
{
    public DateTime CreatedDateTime { get; private set; }
    public Address Address { get; private set; } //[Owned] olarak dbcontext'de işaretleyeceğiz.
    /*
     * Ef core'da belirtmediğimiz sürece Order tablosu içerisinde Address içerisindeki her property'e bir satır oluşturur.
     * Biz burada Address i farklı bir tabloda tut ve order ile ilişkilendir seçeneğini de kullanabiliriz..
     * EF CORE'da böyle tiplere Owned Entity Types denir.
    */
    public string BuyerId { get;private set; }

    private readonly List<OrderItem> _orderItems;
    //EfCore içerisinde read/write işlemlerini property'den ziyade bir field'dan gerçekleştiriyorsak "Backing Field"(Microsoft) olarak adlandırırız.
    //Order üzerinden orderItem'a data eklenmesin method kullanılsın amacıyla yapılır.
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;//sadece okunabilir şekilde data açıldı.


    //Kapsülleme işlemi yaptık.


    public Order(Address address, string buyerId)
    {
        Address = address;
        BuyerId = buyerId;
        _orderItems = new List<OrderItem>();
        CreatedDateTime = DateTime.UtcNow;
    }
    public void AddOrderItem(string productid,string productname,decimal price,string pictureurl) 
    {
        var existProduct=_orderItems.Any(x=>x.ProductId == productid);
        if (!existProduct)
        {
            var orderItem = new OrderItem(productid, productname, pictureurl, price); //OrderItem Ctor'u ile ekledik.
        }
    }

    public decimal GetTotalPrice => _orderItems.Sum(x=>x.Price);
}