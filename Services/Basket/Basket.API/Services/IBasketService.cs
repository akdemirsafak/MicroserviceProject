using Basket.API.Dtos;
using SharedLibrary.Dtos;

namespace Basket.API.Services
{
    public interface IBasketService
    {
        Task<Response<BasketDto>> GetBasket(string userId);
        Task<Response<bool>> SaveOrUpdate(BasketDto request);
        Task<Response<bool>> Delete(string userId);
    }
}
