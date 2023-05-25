namespace Order.Application.Dtos;

public class AddressDto{
    public string Province { get;private set; }
    public string District { get; private set; }
    public string Street { get; private set; }
    public string City { get; private set; }
    public string Line { get; private set; }
    public string ZipCode { get; private set; }
}