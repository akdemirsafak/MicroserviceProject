using SharedLibrary.Dtos;

namespace Discount.API.Services;

public interface IDiscountService
{
    Task<Response<List<DiscountModel>>> GetAll();
    Task<Response<DiscountModel>> GetById(int discountId);
    Task<Response<NoContent>> Save(DiscountModel discountModel);
    Task<Response<NoContent>> Update(DiscountModel discountModel);
    Task<Response<NoContent>> Delete(int discoundId);

    //Sepet sayfasında gelen indirim kodu kullanıcıya mı ait bunu denetleyecek
    Task<Response<DiscountModel>> GetByCodeAndUserId(string code, string userId);
}
