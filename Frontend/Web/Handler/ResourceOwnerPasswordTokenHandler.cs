using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Web.Exceptions;
using Web.Services.Interfaces;

namespace Web.Handler;
public class ResourceOwnerPasswordTokenHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<ResourceOwnerPasswordTokenHandler> _logger;
    private readonly IIdentityService _identityService;

    public ResourceOwnerPasswordTokenHandler(IHttpContextAccessor httpContextAccessor, IIdentityService identityService, ILogger<ResourceOwnerPasswordTokenHandler> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _identityService = identityService;
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",accessToken);
        //SPA kullanmak istesek Interceptor'lar ile çalışacaktık.
        var response= await base.SendAsync(request, cancellationToken);
        if (response.StatusCode==System.Net.HttpStatusCode.Unauthorized)
        {
            var tokenResponse = await _identityService.GetAccessTokenByRefreshToken();
            if (tokenResponse is not null)
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);
                await base.SendAsync(request, cancellationToken);
            }
        }
        if (response.StatusCode==System.Net.HttpStatusCode.Unauthorized)
        {
            //throw exception we will code middleware
            throw new UnAuthorizeException();

        }
        return response;
    }
}
