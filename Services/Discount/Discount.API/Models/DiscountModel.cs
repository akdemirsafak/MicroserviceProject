
[Dapper.Contrib.Extensions.Table("discount")] //Postgresql içerisinde tablo adları genelde küçük harfler olduğu için.
public class DiscountModel
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int Rate { get; set; }
    public string Code { get; set; }    
    public DateTime CreatedTime { get; set; }
}
