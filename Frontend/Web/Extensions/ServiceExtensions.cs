using Web.Handler;
using Web.Services.Interfaces;
using Web.Services;

namespace Web.Extensions;

public static class ServiceExtensions
{
    public static void AddHttpClientServices(this IServiceCollection services,IConfiguration configuration)
    {
        var serviceApiSettings=configuration.GetSection("ServiceApiSettings").Get<ServiceApiSettings>();

        services.AddHttpClient<IClientCredentialTokenService, ClientCredentialTokenService>();

        services.AddHttpClient<ICatalogService, CatalogService>(opt =>
        {
            opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.Catalog.Path}");
        }).AddHttpMessageHandler<ClientCredentialTokenHandler>();

        services.AddHttpClient<IPhotoStockService, PhotoStockService>(opt =>
        {
            opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.PhotoStock.Path}");
        }).AddHttpMessageHandler<ClientCredentialTokenHandler>();

        services.AddHttpClient<IUserService, UserService>(opt =>
        {
            opt.BaseAddress = new Uri(serviceApiSettings.IdentityBaseUri);
        }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>(); //UserService çağırıldığında requestin header'ine token ekleyecek.

        services.AddHttpClient<IBasketService, BasketService>(opt =>
        {
            opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.Basket.Path}");
        }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

        services.AddHttpClient<IDiscountService, DiscountService>(opt =>
        {
            opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.Discount.Path}");
        }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();
        services.AddHttpClient<IPaymentService, PaymentService>(opt =>
        {
            opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.Payment.Path}");
        }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

    }
}
