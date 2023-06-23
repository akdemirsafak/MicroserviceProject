using Web.Models;
using Web.Services.Interfaces;

namespace Web.Services;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;

    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<UserViewModel> GetUserAsync()
    {
        return await _httpClient.GetFromJsonAsync<UserViewModel>("/api/user/getuser"); //case sensitive değil. BURASI IDENTITYSERVER4 CONTROLLER'DA BELIRLEDIGIMIZ ENDPOINT
        //Burada req. token eklemedik.Bunun yerine handle edeceğiz ve otomatik eklemesini sağlayacağız. 
    }
}
