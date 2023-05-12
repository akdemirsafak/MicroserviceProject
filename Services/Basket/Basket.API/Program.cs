using Basket.API.Services;
using Basket.API.Settings;
using Microsoft.Extensions.Options;
using SharedLibrary.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IBasketService,BasketService>();
builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection("RedisSettings")); //Options Pattern
builder.Services.AddSingleton<RedisService>(serviceProvider =>
{
    var redisSettings= serviceProvider.GetRequiredService<IOptions<RedisSettings>>().Value;
    var redis = new RedisService(redisSettings.Host, redisSettings.Port);
    redis.Connect();
    return redis;
});

builder.Services.AddHttpContextAccessor(); //SharedLib'de userId'ye ulaşmak amacıyla oluşturduğumuz SharedIdentityService'e erişebilmek için dahil ettik.
builder.Services.AddScoped<ISharedIdentityService, SharedIdentityService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
