using Order.Domain.Core;

namespace Order.Domain.OrderAggregate;

public class Address : ValueObject
{
    //Value objectleri korumak için set's private olarak ayarlandı.
    public string Province { get;private set; }
    public string District { get; private set; }
    public string Street { get; private set; }
    public string Line { get; private set; }
    public string ZipCode { get; private set; }

    public Address(string province, string district, string street, string line, string zipCode)
    {
        Province = province;
        District = district;
        Street = street;
        Line = line;
        ZipCode = zipCode;
    }
    //Ekstra bir işlem yapmak istiyorsak methodlar tanımlayarak yapmalıyız.Set'ler private edildiği için artık dışarıdan düzenlenemez.Constructor kullanmalı.

    public void SetZipCode(string code)
    {
        //Business kuralları...
    }

    protected override IEnumerable<object> GetEqualityComponents() 
    {
        //yield için bir satır açalım.
        yield return Province;
        yield return District;
        yield return Street;
        yield return ZipCode;
        yield return Line;
    }
}