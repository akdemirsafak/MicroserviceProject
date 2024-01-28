using MicroserviceGateway.DelegateHandlers;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAuthentication()
    .AddJwtBearer("GatewayAuthenticationScheme", opt => //Bu scheme name config dosyasında hangi root'a eklersek o token ile korunacak.
{
    opt.Authority = builder.Configuration["IdentityServerURL"];
    opt.Audience = "resource_gateway";
    opt.RequireHttpsMetadata = false;
});
builder.Services.AddHttpClient<TokenExchangeDelegateHandler>();
builder.Services.AddHttpContextAccessor();

builder.Services
    .AddOcelot()
    .AddDelegatingHandler<TokenExchangeDelegateHandler>(); //TokenExchangeDelegateHandler'ı ekliyoruz.

builder.Configuration.AddJsonFile($"configuration.{builder.Environment.EnvironmentName}.json")
    
    .AddEnvironmentVariables();
//Json dosyasını seçiyoruz. Hangi ortamda up ise o config dosyasını alacak. 
// TODO Bu kısımı .net7 ye uyduralım.
var app = builder.Build();

app.UseAuthorization();
await app.UseOcelot();

app.Run();
