using IdentityModel;
using IdentityServer4.Validation;
using MicroserviceProject.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroserviceProject.IdentityServer.Services
{
    public class IdentityResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        //Bu class Her Resource Owner türündeki istek bu class'a gelecek ve ilgili user,password varsa token dönecek.
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityResourceOwnerPasswordValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            // Identity üyelik sistemi kullanmak zorunda değiliz.Custom tablolarımızla da çalışabiliriz.
            var existUser = await _userManager.FindByEmailAsync(context.UserName); //Senaryo gereği email ile çalışacağız fakat context'den username geliyor.
            if (existUser == null) //Bu kısım zorunlu değil sadece return de edilebilir.
            {
                //IdentityServer'ın döneceği bir response bulunmaktadır.Biz bu response'a ek yapacağız.CustomResponse methodunu kullanacağız.
                var errors=new Dictionary<string, object>();
                errors.Add("errors", new List<string> { "Email veya şifre yanlış" });
                context.Result.CustomResponse = errors;
                return;
            }
            var checkPassword= await _userManager.CheckPasswordAsync(existUser, context.Password);
            if (!checkPassword)
            {
                var errors = new Dictionary<string, object>();
                errors.Add("errors", new List<string> { "Email veya şifre yanlış" });
                context.Result.CustomResponse = errors;
                return;
            }
            context.Result = new GrantValidationResult(existUser.Id.ToString(), OidcConstants.AuthenticationMethods.Password); //id,Grant type Resource Owner Credential
            //kullanıcı bilgilerinin doğru olduğunu ve token dönülmesi gerektiğini söyledik.
        }
    }
}
