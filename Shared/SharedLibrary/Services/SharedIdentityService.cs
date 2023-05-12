using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace SharedLibrary.Services
{
    public class SharedIdentityService : ISharedIdentityService
    {
        private IHttpContextAccessor _httpContextAccessor; //User claimlerine erişmek için

        public SharedIdentityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId => _httpContextAccessor.HttpContext.User.FindFirstValue("sub"); //FindFirst("sub").Value
    }
}
