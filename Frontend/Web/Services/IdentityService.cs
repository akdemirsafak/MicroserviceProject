using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using SharedLibrary.Dtos;
using System.Globalization;
using System.Security.Claims;
using System.Text.Json;
using Web.Models;
using Web.Services.Interfaces;

namespace Web.Services;

public class IdentityService : IIdentityService
{
    private readonly HttpClient _httpClient; //Identity Server ve microservices e istek yapabilmek için
    private readonly IHttpContextAccessor _httpContextAccessor; //Cookie'ye erişim için
    private readonly ClientSettings _clientSettings;
    private readonly ServiceApiSettings _serviceApiSettings;

    public IdentityService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, 
        IOptions<ClientSettings> clientSettings, IOptions<ServiceApiSettings> serviceApiSettings) //Program.cs de singleton olarak da ayarlanabilirdi.
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
        _clientSettings = clientSettings.Value;
        _serviceApiSettings = serviceApiSettings.Value;
    }

    public async Task<TokenResponse> GetAccessTokenByRefreshToken()
    {

        throw new NotImplementedException();
    }

    public Task RevokeRefreshToken()
    {
        throw new NotImplementedException();
    }

    public async Task<Response<bool>> SignInAsync(SignInInput signInInput)
    {
        var discovery= await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
        {
            Address=_serviceApiSettings.BaseUri,
            Policy= new DiscoveryPolicy{RequireHttps=false }
        }); //IdentiyModel'den client için yazılmış. Default olarak https'e istek yapar.

        if (discovery.IsError)
        {
            throw discovery.Exception;
        }

        var passwordTokenRequest= new PasswordTokenRequest
        {
            ClientId=_clientSettings.WebClientForUser.ClientId,
            ClientSecret=_clientSettings.WebClientForUser.ClientSecret,
            UserName = signInInput.Email,
            Password= signInInput.Password,
            Address= discovery.TokenEndpoint //İstek yapacağı endpoint
        }; //Burada PasswordTokenRequest'i Resource Owner granttype'ımıza göre seçtik.

        var token=await _httpClient.RequestPasswordTokenAsync(passwordTokenRequest);

        if (token.IsError)
        {
            //ClientId ve ClientSecret sabit değerlerdir. Eğer bir hata varsa kullanıcı adı/email ve/veya password hatalıdır.

            var responseContent= await token.HttpResponse.Content.ReadAsStringAsync();
            var errorDto= JsonSerializer.Deserialize<ErrorDto>(responseContent,new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive=true //Case sensitive iptal
            });
            return Response<bool>.Fail(errorDto.Errors, 400);
        }
        //Token içerisinde rol,email ve öteki bilgileri userinfo endpointine atacağımız req'den gelsin ki elimizdeki token'ı şişirmeyelim.
        var userInfoRequest= new UserInfoRequest{
            Token = token.AccessToken,
            Address = discovery.TokenEndpoint 
        };
        var userInfo=await _httpClient.GetUserInfoAsync(userInfoRequest);
        if (userInfo.IsError)
        {
            throw userInfo.Exception;
        }
        //Elimizde userInfo var.Cookie'ye ekleyelim ki claim bazlı yetkilendirme yapabilelim.

        ClaimsIdentity claimsIdentity= new(userInfo.Claims,
            CookieAuthenticationDefaults.AuthenticationScheme,
            "name","role"
            ); //Sadece tek bir üyelik sistemimiz olduğu için AuthenticationScheme ile geçtik.
        //Herhangi bir yerde kullanıcının username'ini almak istediğimizde HttpContext.User.Identity.Name üzerinden alıyoruz.Bu değeri almak için cookie'den hangi değeri okuyacağımızı "name" keywordu ile belirledik.Bu zaten identityserver4.readthedocs.io'da endpoints/userInfo example'ında mevcut
        //Requestten gelen rolleri de role keyword'unden okuyacağız.
        ClaimsPrincipal claimsPrincipal=new(claimsIdentity);

        var authenticationProperties= new AuthenticationProperties(); //Accesstoken ve refresh token'ı cookie de tutacağız.
        authenticationProperties.StoreTokens(new List<AuthenticationToken> { 
            new AuthenticationToken{
                Name=OpenIdConnectParameterNames.AccessToken,
                Value=token.AccessToken },
             new AuthenticationToken{
                Name=OpenIdConnectParameterNames.RefreshToken,
                Value=token.RefreshToken },
              new AuthenticationToken{
                Name=OpenIdConnectParameterNames.ExpiresIn,
                Value=DateTime.Now.AddSeconds(token.ExpiresIn).ToString("O",CultureInfo.InvariantCulture) } 
        }); //Buradaki name value'leri sabitlerden alalım OpenidConnect

        //oAuth2.0 yetkilendirmeyle ilgili bir protokoldür
        //OpenIdConnect ise authentication ile ilgilidir.
        //Eğer kullanıcının login olması için IdentityServer'a yönlendirecek olsaydık:
        //Microsoft.AspNetCore.Authentication.OpenIdConnect package'a ihtiyacımız olacaktı.
        //Microsoft.IdentityModel.Protocols.OpenIdConnect paketini projemize dahil ettik.

        authenticationProperties.IsPersistent = signInInput.RememberMe;

        await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,claimsPrincipal,authenticationProperties);

        return Response<bool>.Success(200);




    }
}
