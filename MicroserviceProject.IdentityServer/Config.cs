// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;

namespace MicroserviceProject.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        {
            new ApiResource("resource_catalog"){Scopes={"catalog_fullpermission"}},
            new ApiResource("resource_photo_stock"){Scopes={"photo_stock_fullpermission"}},
            new ApiResource(IdentityServerConstants.LocalApi.ScopeName),
            new ApiResource("resource_basket"){Scopes={"basket_fullpermission"}},
            new ApiResource("resource_discount"){Scopes={"discount_fullpermission","discount_read","discount_write"}},
            new ApiResource("resource_order"){Scopes={"order_fullpermission"}},
            new ApiResource("resource_fakepayment"){Scopes={"fakepayment_fullpermission"}},
            new ApiResource("resource_gateway"){Scopes={"gateway_fullpermission"}}
        }; // audience'lara karşılık gelecek.
        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                       new IdentityResources.Email(),
                       new IdentityResources.OpenId(), //kullanıcının id email ve password gönderildiğinde jwt almak istiyorsak mutlaka token içerisinde(jwt payloadında) subject keyword'ünün dolu olması gerekir.OpenId mutlaka olmalı OpenIdConnect Protocol'ünün zorunlu kıldığı alandır.
                       new IdentityResources.Profile(), //kullanıcı profil bilgileri address vs
                       new IdentityResource(){Name="roles",DisplayName="Roles" ,Description="Kullanıcı rolleri",UserClaims=new[]{ "role"} } //Kendi claim'imizi oluşturuyoruz yukarıdakiler hazır claimler
                //new IdentityResources.OpenId(),
                //new IdentityResources.Profile(),
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("catalog_fullpermission","CatalogApi için full erişim"),
                new ApiScope("photo_stock_fullpermission","PhotoStockApi için full erişim"),
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName), //Identity Server'ın kendisine istek yapabilmek için IdentityServer'ın kendi sabiti. IdentityServer'ı da versek kabul eder.
                new ApiScope("basket_fullpermission","Basket Api için full erişim"),
                new ApiScope("discount_fullpermission","Discount için full erişim."),
                new ApiScope("discount_read","Discount için read."),
                new ApiScope("discount_write","Discount için write."),
                new ApiScope("order_fullpermission","OrderApi için full erişim"),
                new ApiScope("fakepayment_fullpermission","Fakepayment için full erişim"),
                  new ApiScope("gateway_fullpermission","Gateway için full erişim")
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client()
                {
                    ClientName = "Asp.Net Mvc Projesi",
                    ClientId="WebMvcClient",
                    ClientSecrets={new Secret("secret".Sha256())},
                    AllowedGrantTypes=GrantTypes.ClientCredentials, //Refresh token barındırmaz.
                    AllowedScopes={"catalog_fullpermission",
                        "photo_stock_fullpermission",
                        "gateway_fullpermission", 
                        IdentityServerConstants.LocalApi.ScopeName} //Bu scope'da belirlediğimiz clientId ve Secret ile hangi api'lara istek yapılabileceğini burada belirtiyoruz.
                },
                 new Client()
                {
                    ClientName = "Asp.Net Mvc Projesi",
                    ClientId="WebMvcClientForUser",
                    AllowOfflineAccess=true,
                    ClientSecrets={new Secret("secret".Sha256())},
                    AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,//ResourceOwnerPasswordAndClientCredentials kullanırsak refresh token kullanamayız.
                    AllowedScopes={"basket_fullpermission",
                         "discount_fullpermission",
                         "gateway_fullpermission", //Kullanıcı için bir token alındığında gidilebilecek mikroservisler.
                         "order_fullpermission",
                         "fakepayment_fullpermission", 
                         IdentityServerConstants.StandardScopes.Email,
                         IdentityServerConstants.StandardScopes.OpenId,
                         IdentityServerConstants.StandardScopes.Profile,
                         IdentityServerConstants.StandardScopes.OfflineAccess,
                         IdentityServerConstants.LocalApi.ScopeName, //API'nin kendisine istek yapabilmesi için.
                         "roles"}, //Refresh token dönebilmemiz için OfflineAccess'de ekledik.
                    //**** OfflineAccess Kullanıcı offline olsa bile kullanıcı adına bir refresh token göndererek kullanıcı için yeni bir accesstoken almamıza olanak verir.
                    AccessTokenLifetime=1*60*60, //Default 1 saattir. saniye türünden belirtiyoruz. Access token 1 saat geçerli.
                    RefreshTokenExpiration=TokenExpiration.Absolute, //Refresh token süresini istediğimiz zaman uzatacak mıyız ayarı.Yaptığımız ayar sabite denk gelir.
                    AbsoluteRefreshTokenLifetime=(int)(DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds, //Refresh token 60 gün
                    RefreshTokenUsage=TokenUsage.ReUse //Tekrar kullanılabilir Refresh token
                    // ! Her accesstoken alımında yeni bir refresh token da alınacaktır!.
                 }
                

                //// interactive client using code flow + pkce
                //new Client
                //{
                //    ClientId = "interactive",
                //    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                //    AllowedGrantTypes = GrantTypes.Code,

                //    RedirectUris = { "https://localhost:44300/signin-oidc" },
                //    FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                //    PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

                //    AllowOfflineAccess = true,
                //    AllowedScopes = { "openid", "profile", "scope2" }
                //},
            };
    }
}