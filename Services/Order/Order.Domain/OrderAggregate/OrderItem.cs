namespace Order.Domain.OrderAggregate;

public class OrderItem
{
    public string ProductId { get; private set; }
    public string ProductName { get; private set; }
    public string PictureUrl { get; private set; }
    public decimal Price { get; private set;}
    ///DDD'da burada OrderId ve Order tanımlanmaz.Burada orderId de alırsak tek başına orderItem eklenebilir hale gelir. 
    //1:n için burada property tanımlamamamıza rağmen ef core bunu tanımlayacak.Buna Shadow property denir.Db'de bir sütun olan entity'de karşılığı olmayan property'lerdir.
    public OrderItem(string productId, string productName, string pictureUrl, decimal price)
    {
        ProductId = productId;
        ProductName = productName;
        PictureUrl = pictureUrl;
        Price = price;
    }
    public void SetOrderItem(string productName,string pictureUrl, decimal price)
    {
        ProductName = productName;
        PictureUrl = pictureUrl;
        Price = price;
    }
}