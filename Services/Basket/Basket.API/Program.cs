using Basket.API.Services;
using Basket.API.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Options;
using SharedLibrary.Services;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.




var requirePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub"); //sub alanının nameidentifier olarak görünmesinin önüne geçiyoruz.
//Burada kesinlikle bir sub alanı bekliyoruz bu sebeple yukarıdaki policy'i yazıyoruz.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(configs =>
{
    configs.Authority = builder.Configuration["IdentityServerURL"];
    configs.Audience = "resource_basket";
    configs.RequireHttpsMetadata = false;
});


builder.Services.AddScoped<IBasketService, BasketService>();
builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection("RedisSettings")); //Options Pattern
builder.Services.AddSingleton<RedisService>(serviceProvider =>
{
    var redisSettings = serviceProvider.GetRequiredService<IOptions<RedisSettings>>().Value;
    var redis = new RedisService(redisSettings.Host, redisSettings.Port);
    redis.Connect();
    return redis;
});

builder.Services.AddHttpContextAccessor(); //SharedLib'de userId'ye ulaşmak amacıyla oluşturduğumuz SharedIdentityService'e erişebilmek için dahil ettik.
builder.Services.AddScoped<ISharedIdentityService, SharedIdentityService>();


builder.Services.AddControllers(opt =>
{
    opt.Filters.Add(new AuthorizeFilter(requirePolicy)); //Mutlaka authorize olmuş bir kullanıcı olarak. 
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
