using IdentityModel.Client;
using SharedLibrary.Dtos;
using Web.Models;

namespace Web.Services.Interfaces;

public interface IIdentityService
{
    Task<Response<bool>> SignInAsync(SignInInput signInInput);
    Task<TokenResponse> GetAccessTokenByRefreshToken();
    Task RevokeRefreshToken();

}
