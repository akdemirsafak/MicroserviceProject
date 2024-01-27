using IdentityServer4.Models;
using IdentityServer4.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace MicroserviceProject.IdentityServer.Services
{
    public class TokenExchangeExtensionGrantValidator : IExtensionGrantValidator
    {
        public string GrantType => "urn:ietf:params:oauth:grant-type:token-exchange"; //GrantType'i belirtiyoruz.oAuth2.0'ın belirlediği isim standartı bu şekilde. İstediğimiz isimlendirmeyi kullanabiliriz.

        private readonly ITokenValidator _tokenValidator;

        public TokenExchangeExtensionGrantValidator(ITokenValidator tokenValidator)
        {
            _tokenValidator = tokenValidator;
        }

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            //Gelen token'ı doğrulayalım ve istediğimiz claimleri ekleyelim.(İstek yapılabilecek api'ları)
            var requestRaw = context.Request.Raw.ToString();
            var token = context.Request.Raw.Get("subject_token"); //subject_token : token exchange ile gelen token.
            if (string.IsNullOrEmpty(token))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "token missing");
            }

            var tokenValidationResult = await _tokenValidator.ValidateAccessTokenAsync(token);
            
            if(tokenValidationResult.IsError)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "token invalid");
            }

            var subjectClaim = tokenValidationResult.Claims.FirstOrDefault(x => x.Type == "sub").Value;
            if(subjectClaim is null)
            {
                   context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "token does not contain sub value");
            }

            context.Result=new GrantValidationResult(subjectClaim, "access_token", tokenValidationResult.Claims);

        }
    }
}
