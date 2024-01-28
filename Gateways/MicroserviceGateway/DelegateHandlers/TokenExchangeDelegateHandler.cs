
using IdentityModel.Client;

namespace MicroserviceGateway.DelegateHandlers
{
    public class TokenExchangeDelegateHandler :DelegatingHandler
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private string _accessToken;

        public TokenExchangeDelegateHandler(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        } 
        private async Task<string> GetAccessTokenAsync(string requestToken)
        {
            if (!String.IsNullOrEmpty(_accessToken))
                return _accessToken;

            var discoveryEndpoint= await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _configuration["IdentityServerURL"],
                Policy = new DiscoveryPolicy {RequireHttps = false}
            });
            if (discoveryEndpoint.IsError)
            {
                throw new Exception(discoveryEndpoint.Error);
            }
            TokenExchangeTokenRequest tokenExchangeTokenRequest = new TokenExchangeTokenRequest
            {
                Address = discoveryEndpoint.TokenEndpoint,
                ClientId = _configuration["ClientId"],
                ClientSecret = _configuration["ClientSecret"],
                GrantType = _configuration["TokenGrantType"],
                SubjectToken = requestToken,
                SubjectTokenType = "urn:ietf:params:oauth:token-type:access_token",
                Scope = "openid discount_fullpermission fakepayment_fullpermission"
                //Parameters =
                //{
                //    {"subject_token", requestToken},
                //    {"subject_token_type", "urn:ietf:params:oauth:token-type:access_token"},
                //    {"scope", "openid profile microservice1.fullaccess"}  
                //}
            };

            var tokenResponse= await _httpClient.RequestTokenExchangeTokenAsync(tokenExchangeTokenRequest);
            
            if (tokenResponse.IsError)
                throw new Exception(tokenResponse.Error);

            _accessToken = tokenResponse.AccessToken;
            return _accessToken;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var requestToken = request.Headers.Authorization.Parameter;
            var newAccessToken = await GetAccessTokenAsync(requestToken);

            request.SetBearerToken(newAccessToken);
            
            return await base.SendAsync(request, cancellationToken);
        }

       
    }
}
